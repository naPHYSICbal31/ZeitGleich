using UnityEngine;
using System.Collections;

public class GhostItem : MonoBehaviour
{
    [Header("Settings")]
    public float duration = 10f;
    public float ghostTransparency = 0.5f; // 50% invisible

    private SpriteRenderer mySprite;
    private Collider2D myCollider;

    void Start()
    {
        // Grab our own components so we can hide/show ourselves
        mySprite = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only run if the player touches us AND we are currently "active"
        if (other.CompareTag("Player") && mySprite.enabled)
        {
            StartCoroutine(GhostCycle(other.gameObject));
        }
    }

    IEnumerator GhostCycle(GameObject player)
    {
        // === STEP 1: HIDE THE FIRE ===
        // We do NOT destroy the object, we just turn off visuals and physics
        mySprite.enabled = false;
        myCollider.enabled = false;

        // === STEP 2: ACTIVATE GHOST MODE ON PLAYER ===
        SpriteRenderer playerSprite = player.GetComponent<SpriteRenderer>();
        int playerLayer = player.layer;
        int wallLayer = LayerMask.NameToLayer("GhostWall");

        // Turn OFF collision between Player and GhostWalls
        Physics2D.IgnoreLayerCollision(playerLayer, wallLayer, true);

        // Visual Effect: Fade Player
        if (playerSprite != null)
        {
            Color c = playerSprite.color;
            c.a = ghostTransparency;
            playerSprite.color = c;
        }

        // === STEP 3: WAIT (The Duration) ===
        yield return new WaitForSeconds(duration);

        // === STEP 4: DEACTIVATE GHOST MODE ===
        // Turn ON collision again
        Physics2D.IgnoreLayerCollision(playerLayer, wallLayer, false);

        // Visual Effect: Restore Player
        if (playerSprite != null)
        {
            Color c = playerSprite.color;
            c.a = 1f;
            playerSprite.color = c;
        }

        // === STEP 5: RESPAWN THE FIRE ===
        mySprite.enabled = true;
        myCollider.enabled = true;
    }
}