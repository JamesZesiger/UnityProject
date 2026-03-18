using UnityEngine;
using UnityEngine.AI;

public class ImpAI_NavMesh : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Ranges")]
    public float chaseDistance = 10f;
    public float stopDistance = 1.5f;

    [Header("Attack")]
    public float attackCooldown = 1.5f;
    public int damage = 10;
    private float nextAttackTime;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    private NavMeshAgent agent;

    void Awake()
    {
        if (player == null)
            player = Camera.main.transform;

        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= chaseDistance)
        {
            agent.SetDestination(player.position);

            if (distance <= stopDistance)
            {
                agent.ResetPath();
                TryAttack();
            }
        }
        else
        {
            Patrol();
        }
    }

    void TryAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;

            Debug.Log("Imp attacks for " + damage + " damage!");

            PlayerHealth health = player.GetComponent<PlayerHealth>();
            if (health != null)
                health.TakeDamage(damage);
        }
    }

    void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }
}