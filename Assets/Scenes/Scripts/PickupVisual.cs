using UnityEngine;

public class PickupVisual : MonoBehaviour
{
    public float rotateSpeed = 90f;
    public float bobSpeed = 2f;
    public float bobHeight = 0.25f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Rotate
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

        // Bob up/down
        float newY = startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}