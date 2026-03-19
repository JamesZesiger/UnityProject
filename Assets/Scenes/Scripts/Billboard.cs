using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        Vector3 cameraPos = Camera.main.transform.position;

        // Keep the same Y level so it only rotates horizontally
        cameraPos.y = transform.position.y;

        transform.LookAt(cameraPos);

        // Optional: flip if your sprite faces backward
        transform.Rotate(0, 180, 0);
    }
}