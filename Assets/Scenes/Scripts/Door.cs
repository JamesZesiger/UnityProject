using UnityEngine;

public class Door : InteractableObject
{
    [Header("Settings")]
    public float speed = 5f;
    public float moveDistance = 5f;
    public Vector3 openDirection = Vector3.up;

    private bool isOpen = false;
    private bool isMoving = false;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    void Start()
    {
        startPosition = transform.position;

        // Normalize direction so distance is consistent
        openDirection = openDirection.normalized;
    }

    void Update()
    {
        if (!isMoving) return;

        Vector3 target = isOpen ? startPosition : startPosition + openDirection * moveDistance;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            transform.position = target;
            isMoving = false;
            isOpen = !isOpen;
        }
    }

    protected override void Interact()
    {
        if (!isMoving)
            isMoving = true;
    }
}