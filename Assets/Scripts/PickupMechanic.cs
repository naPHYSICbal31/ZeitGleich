using UnityEngine;

public class PickupMechanic : MonoBehaviour
{
    [Header("Settings")]
    public Transform holdPoint;        // Drag your 'HoldPoint' here
    public KeyCode interactKey = KeyCode.F;
    public float pickupRange = 1.5f;   // How close you need to be
    public LayerMask pickupLayer;      // Set this to a layer named "Pickup"

    private GameObject heldObject;     // What are we holding right now?
    AudioManagerL2 audioManager;
    void Awake()
{
    // 1. FIND THE AUDIO MANAGER (Misssing Line!)
    // If you don't do this, the game crashes when you press F.
    GameObject audioObj = GameObject.FindGameObjectWithTag("Audio");
    if (audioObj != null)
    {
        audioManager = audioObj.GetComponent<AudioManagerL2>();
    }
    else
    {
        Debug.LogError("CRITICAL: No object with tag 'Audio' found in the scene!");
    }

    // 2. Auto-find the HoldPoint (Your existing fix)
    if (holdPoint == null)
    {
        Transform foundChild = transform.Find("HoldPoint");
        if (foundChild != null)
        {
            holdPoint = foundChild;
        }
        else
        {
            GameObject newPoint = new GameObject("HoldPoint");
            newPoint.transform.SetParent(this.transform);
            holdPoint = newPoint.transform;
            Debug.LogWarning("Auto-created missing HoldPoint for Player.");
        }
    }
}
    void Update()
    {
        // 1. Flip the HoldPoint based on player direction
        // (Assuming you use transform.localScale.x for facing)
        if (transform.localScale.x > 0)
            holdPoint.localPosition = new Vector3(Mathf.Abs(holdPoint.localPosition.x), holdPoint.localPosition.y, 0);
        else
            holdPoint.localPosition = new Vector3(-Mathf.Abs(holdPoint.localPosition.x), holdPoint.localPosition.y, 0);

        // 2. Input Check
        if (Input.GetKeyDown(interactKey))
        {
            if (heldObject == null)
            {
                TryPickup();
                audioManager.PlaySFX(audioManager.acquire);
            }
            else
            {
                DropObject();
                audioManager.PlaySFX(audioManager.place);
            }
        }
    }

    void TryPickup()
    {
        // Draw an invisible circle to detect items
        Collider2D foundItem = Physics2D.OverlapCircle(transform.position, pickupRange, pickupLayer);

        if (foundItem != null)
        {
            heldObject = foundItem.gameObject;

            heldObject.GetComponent<Collider2D>().enabled = false;
            
            // === PHYSICS OFF ===
            // We turn off gravity so it doesn't fall out of our hands
            Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.isKinematic = true; // Stops physics
                rb.velocity = Vector2.zero; // Stops moving
            }

            // === PARENTING ===
            // This makes the object stick to the player automatically
            heldObject.transform.SetParent(holdPoint);
            heldObject.transform.localPosition = Vector3.zero; // Snap to center
            heldObject.transform.localRotation = Quaternion.identity; // Reset rotation
        }
    }

    void DropObject()
    {
        if (heldObject == null) return;

        // 1. RE-PARENT TO THE CORRECT TIMELINE
        // Check which timeline is currently active
        GameObject past = GameObject.Find("TIMELINE_PAST");
        
        if (past != null && past.activeInHierarchy)
        {
            heldObject.transform.SetParent(past.transform);
        }
        else
        {
            // If Past is off, we must be in Future (rare case, but safe)
            GameObject future = GameObject.Find("TIMELINE_FUTURE");
            if (future != null) heldObject.transform.SetParent(future.transform);
        }

        // 2. PHYSICS ON
        Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = false; 
        }

        heldObject.GetComponent<Collider2D>().enabled = true;
        heldObject = null; 
    }

    // Visualization for Debugging (Draws the range circle)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}