using UnityEngine;

public class Door : InteractableObject
{
    [Header("Settings")]
    public float speed = 5f;
    public float moveDistance = 5f;
    private bool isOpen = false;
    private bool isMoving = false;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    private void Update() 
    {
        if (isOpen && isMoving)
            MoveDoor(startPosition.y, false); 
        if (!isOpen && isMoving)
            MoveDoor(startPosition.y + moveDistance, true);

    }
    protected override void Interact()
    {
        if (isOpen)
        {
            isMoving = true;  
        }
        else if (!isOpen)
        {
            isMoving = true;  
        }
    }

    void MoveDoor(float targetY, bool openState)
    {
        float newY = Mathf.MoveTowards(
            transform.position.y,
            targetY,
            speed * Time.deltaTime
        );

        transform.position = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );

        if (Mathf.Approximately(transform.position.y, targetY))
        {
            transform.position = new Vector3(
                transform.position.x,
                targetY,
                transform.position.z
            );

            isMoving = false;
            isOpen = openState;
        }
    }

}