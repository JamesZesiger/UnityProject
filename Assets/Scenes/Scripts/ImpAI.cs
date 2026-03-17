using UnityEngine;

public class ImpAI : MonoBehaviour
{
    [Header("Target & Movement")]
    public Transform player;
    public float moveSpeed = 3f;
    public float chaseDistance = 10f;
    public float stopDistance = 1.5f;

    [Header("Attack")]
    public float attackCooldown = 1.5f;
    public int damage = 10;
    private float nextAttackTime;

    [Header("Components")]
    public Transform visual; // Visual child
    private Rigidbody rb; // optional if you use physics

    [Header("Patrol")]
    public Transform[] patrolPoints; // assign in inspector
    public float patrolSpeed = 2f;
    private int currentPatrolIndex = 0;
    private bool patrolForward = true;


    void Awake()
    {
        if (player == null)
            player = Camera.main.transform;

        rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;
    }

    void Update()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0;
        float distance = dir.magnitude;

        Vector3 moveDir = Vector3.zero; // direction we will move this frame

        if (distance <= chaseDistance && distance > stopDistance)
        {
            // Chase player
            moveDir = dir.normalized;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
        else if (distance > chaseDistance)
        {
            // Patrol
            moveDir = GetPatrolDirection();
            transform.position += moveDir * patrolSpeed * Time.deltaTime;
        }

        if (distance <= stopDistance)
        {
            TryAttack();
        }

        // Rotate root toward movement direction only
        if (moveDir.sqrMagnitude > 0.001f)
        {
            transform.forward = moveDir;
        }
    }

    void TryAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            Debug.Log("Imp attacks for " + damage + " damage!");

            // You can add damage logic here, e.g., player.GetComponent<Health>().TakeDamage(damage);
        }
    }

    Vector3 GetPatrolDirection()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return Vector3.zero;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        Vector3 dir = targetPoint.position - transform.position;
        dir.y = 0;

        // Check if reached current patrol point
        float distanceToPoint = dir.magnitude;
        if (distanceToPoint < 0.2f) // small threshold
        {
            // Advance to next node
            if (patrolForward)
            {
                currentPatrolIndex++;
                if (currentPatrolIndex >= patrolPoints.Length)
                {
                    currentPatrolIndex = patrolPoints.Length - 2;
                    patrolForward = false;
                }
            }
            else
            {
                currentPatrolIndex--;
                if (currentPatrolIndex < 0)
                {
                    currentPatrolIndex = 1;
                    patrolForward = true;
                }
            }

            targetPoint = patrolPoints[currentPatrolIndex];
            dir = targetPoint.position - transform.position;
            dir.y = 0;
        }

        if (dir.sqrMagnitude < 0.001f)
            return Vector3.zero;

        return dir.normalized;
    }

    void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        Vector3 dir = targetPoint.position - transform.position;
        dir.y = 0;

        // Move toward patrol point
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, patrolSpeed * Time.deltaTime);

        // Rotate root to face patrol direction
        if (dir.sqrMagnitude > 0.01f)
            transform.forward = dir.normalized;

        // Check if reached point
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            // Move to next point
            if (patrolForward)
            {
                currentPatrolIndex++;
                if (currentPatrolIndex >= patrolPoints.Length)
                {
                    currentPatrolIndex = patrolPoints.Length - 2;
                    patrolForward = false;
                }
            }
            else
            {
                currentPatrolIndex--;
                if (currentPatrolIndex < 0)
                {
                    currentPatrolIndex = 1;
                    patrolForward = true;
                }
            }
        }
    }

}