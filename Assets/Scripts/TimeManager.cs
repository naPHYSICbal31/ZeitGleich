using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [Header("Timeline Setup")]
    public GameObject pastTimeline;
    public GameObject futureTimeline;

    [Header("Transition Settings")]
    public Image curtainPanel;
    public float transitionSpeed = 1f;
    public KeyCode switchKey = KeyCode.E;

    private bool isPast = true;
    private bool isSwitching = false;

    void Start()
    {
        Debug.Log("TimeManager Started. Current State: Past=" + isPast);
        pastTimeline.SetActive(true);
        futureTimeline.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(switchKey) && !isSwitching)
        {
            Debug.Log("1. Input Detected! Starting Switch...");
            StartCoroutine(PerformTimeSwitch());
        }
    }

    IEnumerator PerformTimeSwitch()
    {
        isSwitching = true; 

        // === PHASE 1: FADE IN ===
        Debug.Log("2. Fading to Black...");
        float alpha = 0;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * transitionSpeed;
            SetCurtainAlpha(alpha);
            yield return null;
        }

        // === PHASE 2: SWAP ===
        Debug.Log("3. Screen is Black. Swapping Timelines...");
        SetCurtainAlpha(1f);
        
        isPast = !isPast;
        pastTimeline.SetActive(isPast);
        futureTimeline.SetActive(!isPast);

        // FORCE PHYSICS UPDATE
        Physics2D.SyncTransforms(); 
        yield return new WaitForFixedUpdate();

        // === FIX POSITION ===
        Debug.Log("4. Calling FixPlayerPosition...");
        FixPlayerPosition(); 
        
        // === PHASE 3: FADE OUT ===
        Debug.Log("5. Fading back to Clear...");
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * transitionSpeed;
            SetCurtainAlpha(alpha);
            yield return null;
        }

        SetCurtainAlpha(0f);
        isSwitching = false; 
        Debug.Log("6. Switch Complete.");
    }

    void FixPlayerPosition()
    {
        // DEBUG CHECK 1: FIND PLAYER
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("CRITICAL ERROR: Could not find object with Tag 'Player'!");
            return;
        }
        Debug.Log("Found Player at: " + player.transform.position);

        Collider2D playerCol = player.GetComponent<Collider2D>();
        
        // DEBUG CHECK 2: CHECK LAYER
        int layerMask = LayerMask.GetMask("Ground"); 
        if (layerMask == 0)
        {
            Debug.LogError("CRITICAL ERROR: Layer 'Ground' does not exist! Check Layer settings.");
            return;
        }

        // DEBUG CHECK 3: CHECK COLLISION
        Collider2D hit = Physics2D.OverlapBox(
            playerCol.bounds.center, 
            playerCol.bounds.size * 0.9f, 
            0f, 
            layerMask
        );

        if (hit != null)
        {
            Debug.Log("STUCK! Player is inside: " + hit.gameObject.name);

            RaycastHit2D roofHit = Physics2D.Raycast(
                new Vector2(player.transform.position.x, player.transform.position.y + 20f), 
                Vector2.down, 
                30f, 
                layerMask
            );

            if (roofHit.collider != null)
            {
                Debug.Log("RAYCAST SUCCESS: Roof found at Y=" + roofHit.point.y);
                Vector3 newPos = player.transform.position;
                newPos.y = roofHit.point.y + playerCol.bounds.extents.y + 0.1f; 
                player.transform.position = newPos;
            }
            else
            {
                Debug.LogError("RAYCAST FAILED: Ray shot down but hit nothing. Is the roof directly below X=" + player.transform.position.x + "?");
            }
        }
        else
        {
            Debug.Log("STATUS CLEAR: Player is not inside any 'Ground' object.");
        }
    }

    void SetCurtainAlpha(float a)
    {
        if (curtainPanel != null)
        {
            Color c = curtainPanel.color;
            c.a = Mathf.Clamp01(a);
            curtainPanel.color = c;
        }
    }
}