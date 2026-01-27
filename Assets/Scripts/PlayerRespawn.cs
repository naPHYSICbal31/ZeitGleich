using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 respawnPoint; // Where will we go?
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Default respawn point is where the player starts the game
        respawnPoint = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Case 1: We touched a Checkpoint
        if (other.CompareTag("Checkpoint"))
        {
            // Save this new position as our safe spot
            respawnPoint = other.transform.position;
            Debug.Log("Checkpoint Saved!");
        }

        // Case 2: We fell into the void
        if (other.CompareTag("DeathZone"))
        {
            DieAndRespawn();
        }
    }

    void DieAndRespawn()
    {
        // 1. Stop Physics (So we don't keep falling with momentum)
        rb.velocity = Vector2.zero; 

        // 2. Teleport
        transform.position = respawnPoint;

        // 3. (Optional) Penalties
        // Subtract health, play death sound, etc.
        Debug.Log("Player Died. Respawning...");
    }
}