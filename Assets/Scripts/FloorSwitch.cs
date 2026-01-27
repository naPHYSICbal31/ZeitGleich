using UnityEngine;

public class FloorSwitch : MonoBehaviour
{
    [Header("Settings")]
    public MovingPlatform platformToMove; // Drag your Platform here
    public Sprite pressedSprite;          // Optional: Image of flattened button
    public KeyCode interactKey = KeyCode.F;

    private bool hasBeenPressed = false;

    // This runs every frame while the player is standing inside the switch
    private void OnTriggerStay2D(Collider2D other)
    {
        // 1. Check if it's the Player AND they pressed F AND it hasn't been used yet
        if (other.CompareTag("Player") && Input.GetKeyDown(interactKey) && !hasBeenPressed)
        {
            ActivateSwitch();
        }
    }

    void ActivateSwitch()
    {
        hasBeenPressed = true;
        
        // 1. Activate the Platform
        if (platformToMove != null)
        {
            platformToMove.ActivatePlatform();
        }

        // 2. Visual Feedback (Change sprite)
        if (pressedSprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = pressedSprite;
        }

        Debug.Log("Switch Activated!");
    }
}