using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f; 
    public Vector3 offset;

    void LateUpdate()
    {
        // We only follow X and Y. We force Z to stay at -10.
        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, -10f);
        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        transform.position = smoothedPosition;
    }
}