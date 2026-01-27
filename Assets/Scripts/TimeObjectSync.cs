using UnityEngine;

public class TimeObjectSync : MonoBehaviour
{
    [Header("Link the Future Version")]
    public GameObject futureObject; // Drag the Big Tree here

    void Update()
    {
        if (futureObject != null)
        {
            // 1. Sync Position (Always keep them linked in space)
            futureObject.transform.position = transform.position;

            // 2. CHECK: Is the Sapling being held by the Player?
            // We check if the "Root" parent is the Player
            if (transform.root.CompareTag("Player")) 
            {
                // If held, DISABLE the Big Tree (It can't grow in your pocket)
                futureObject.SetActive(false);
            }
            else
            {
                // If dropped, ENABLE the Big Tree
                // (Note: It will still only be visible if the Future Timeline is active)
                futureObject.SetActive(true);
            }
        }
    }
}