using UnityEngine;

public class LoopingPlatform : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform pointA; // Start point
    public Transform pointB; // End point

    [Header("Settings")]
    public float speed = 3f;
    public float waitTime = 1f; // Pause at each end?

    private Vector3 targetPos;
    private bool isWaiting = false;

    void Start()
    {
        // 1. Detach waypoints so they don't move with the platform
        // (If you forgot to unparent them in the hierarchy, this fixes it)
        if (pointA != null) pointA.parent = null;
        if (pointB != null) pointB.parent = null;

        // Start moving towards B
        targetPos = pointB.position;
    }

    void Update()
    {
        if (isWaiting) return; // Do nothing if pausing

        // 2. Move smoothly towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // 3. Check if we arrived (Distance is very small)
        if (Vector3.Distance(transform.position, targetPos) < 0.05f)
        {
            StartCoroutine(WaitAndSwap());
        }
    }

    // Coroutine to handle the pause
    System.Collections.IEnumerator WaitAndSwap()
    {
        isWaiting = true;
        
        yield return new WaitForSeconds(waitTime);

        // Swap Target: If going to B, switch to A. If A, switch to B.
        if (targetPos == pointB.position)
            targetPos = pointA.position;
        else
            targetPos = pointB.position;

        isWaiting = false;
    }

    // Draw lines in the editor so you can see the path
    void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawWireSphere(pointA.position, 0.2f);
            Gizmos.DrawWireSphere(pointB.position, 0.2f);
        }
    }
}