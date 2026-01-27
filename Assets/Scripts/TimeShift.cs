using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TimeShift : MonoBehaviour
{
    [Header("The Timelines")]
    public GameObject past;
    public GameObject future;

    [Header("Player Settings")]
    public Rigidbody2D playerRb;      // DRAG PLAYER HERE
    public Transform playerTransform; // DRAG PLAYER HERE
    
    [Header("Past Physics (Small & Floaty)")]
    public float pastGravity = 1.5f;   // Lower number = High Jump
    public Vector3 pastScale = new Vector3(0.2f, 0.2f, 1f); // Half size

    [Header("Future Physics (Normal)")]
    public float futureGravity = 2f; // Higher number = Heavy/Normal
    public Vector3 futureScale = new Vector3(.4f, .4f, .4f);   // Normal size

    [Header("Fade Settings")]
    public float fadeDuration = 0.5f; 

    private bool isFuture = false;
    private bool isSwitching = false;
    AudioManagerL2 audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerL2>();
    }


    void Start()
    {
        // 1. Setup the start state (Past = Active)
        InitializeTimeline(past, 1f, true);
        InitializeTimeline(future, 0f, false);
        
        // Apply Past Physics immediately
        UpdatePlayerPhysics(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isSwitching)
        {
            audioManager.PlaySFX(audioManager.transition);
            StartCoroutine(DoCrossFade());
        }
    }

    IEnumerator DoCrossFade()
    {
        isSwitching = true;
        isFuture = !isFuture;

        // --- NEW: Apply Physics Changes Immediately ---
        UpdatePlayerPhysics(isFuture);

        GameObject incoming = isFuture ? future : past;
        GameObject outgoing = isFuture ? past : future;

        incoming.SetActive(true);
        ToggleColliders(incoming, true);
        ToggleColliders(outgoing, false);

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;
            SetAlpha(incoming, Mathf.Lerp(0f, 1f, progress));
            SetAlpha(outgoing, Mathf.Lerp(1f, 0f, progress));
            yield return null;
        }

        SetAlpha(incoming, 1f);
        SetAlpha(outgoing, 0f);
        outgoing.SetActive(false);
        isSwitching = false;
    }

    // --- NEW: The Physics Logic ---
    void UpdatePlayerPhysics(bool futureState)
    {
        if (futureState)
        {
            // Future Mode: Normal Gravity, Normal Size
            playerRb.gravityScale = futureGravity;
            playerTransform.localScale = futureScale;
        }
        else
        {
            // Past Mode: Low Gravity, Small Size
            playerRb.gravityScale = pastGravity;
            playerTransform.localScale = pastScale;
        }
    }

    // --- Helper Functions (Same as before) ---
    void InitializeTimeline(GameObject timeline, float alpha, bool isActive)
    {
        timeline.SetActive(true);
        SetAlpha(timeline, alpha);
        ToggleColliders(timeline, isActive);
        timeline.SetActive(isActive);
    }

    void SetAlpha(GameObject root, float alpha)
    {
        SpriteRenderer[] sprites = root.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in sprites) {
            Color c = sr.color; c.a = alpha; sr.color = c;
        }
        Tilemap[] tiles = root.GetComponentsInChildren<Tilemap>();
        foreach (Tilemap tm in tiles) {
            Color c = tm.color; c.a = alpha; tm.color = c;
        }
    }

    void ToggleColliders(GameObject root, bool state)
    {
        Collider2D[] cols = root.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in cols) col.enabled = state;
    }
}