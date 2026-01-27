using UnityEngine;
using System.Collections;

public class GhostPotion : MonoBehaviour
{
    [Header("Settings")]
    public float duration = 3f; // How long you stay a ghost
    public float ghostSpeed = 5f; // Speed while flying

    private SpriteRenderer potionSprite;
    private Collider2D potionCollider;

    private void Start()
    {
        potionSprite = GetComponent<SpriteRenderer>();
        potionCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ActivateGhostMode(collision.gameObject));
        }
    }

    IEnumerator ActivateGhostMode(GameObject player)
    {
        // 1. Hide the Potion
        potionSprite.enabled = false;
        potionCollider.enabled = false;

        // 2. Get Player Components
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        Collider2D playerCol = player.GetComponent<Collider2D>();
        SpriteRenderer playerSprite = player.GetComponent<SpriteRenderer>();
        PlayerMovement moveScript = player.GetComponent<PlayerMovement>();

        // 3. SAVE ORIGINAL STATE (Important!)
        float originalGravity = rb.gravityScale;
        Color originalColor = playerSprite.color;
        
        // 4. ACTIVATE GHOST MODE
        // Disable Physics Gravity (So you float)
        rb.gravityScale = 0f;
        // Turn collider to Trigger (Pass through walls!)
        playerCol.isTrigger = true; 
        // Visual: Make player transparent
        playerSprite.color = new Color(1f, 1f, 1f, 0.4f); 
        // Stop falling instantly
        rb.velocity = Vector2.zero;

        // 5. Allow "Flying" Movement (Simple override)
        float timer = 0;
        while (timer < duration)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical"); // Now allows UP/DOWN movement
            rb.velocity = new Vector2(x * ghostSpeed, y * ghostSpeed);
            
            timer += Time.deltaTime;
            yield return null;
        }

        // 6. DEACTIVATE (Revert everything)
        rb.gravityScale = originalGravity;
        playerCol.isTrigger = false; // Solid again
        playerSprite.color = originalColor;

        // 7. Destroy Potion
        Destroy(gameObject);
    }
}