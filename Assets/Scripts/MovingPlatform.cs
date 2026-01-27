using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement")]
    public Transform targetPosition; // Drag an empty GameObject here to define "DOWN"
    public float speed = 2f;
    
    private bool isActivated = false;

    void Update()
    {
        if (isActivated && targetPosition != null)
        {
            // Move smoothly towards the target
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, speed * Time.deltaTime);
        }
    }

    // This function will be called by the Switch
    public void ActivatePlatform()
{
    Debug.Log("I have been activated!"); // <--- Add this
    isActivated = true;
}
}