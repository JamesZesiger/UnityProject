using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 5f;
    public float moveDistance = 5f;
    private bool isOpen = false;
    private bool isClosing = false;
    private bool isMoving = false;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Interact()
    {
        if (isOpen)
        {
            close();
        }
        else if (!isOpen)
        {
            open();
        }
    }

    void open()
    {
        isMoving = true;
        float remaining = moveDistance - (transform.position.y - startPosition.y);

        if (remaining > 0f)
        {
            // Move up by speed * deltaTime, but not beyond the target
            float step = Mathf.Min(moveSpeed * Time.deltaTime, remaining);
            transform.Translate(Vector3.up * step, Space.World);
        }
        else
        {
            // Stop movement when target reached
            isMoving = false;
            isOpen = true;
        }
    }

    void close()
    {
        isMoving = true;
        float remaining = moveDistance - (startPosition.y - transform.position.y);
        if (remaining > 0f)
        {
            // Move up by speed * deltaTime, but not beyond the target
            float step = Mathf.Min(moveSpeed * Time.deltaTime, remaining);
            transform.Translate(Vector3.down * step, Space.World);
        }
        else
        {
            // Stop movement when target reached
            isMoving = false;
            isOpen = true;
        }
    }

}