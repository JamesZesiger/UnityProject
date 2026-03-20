using UnityEngine;
using UnityEngine.AI;

public class SoldierAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public EnemyAnimator animator;
    public Transform firePoint;
    public SpriteAnimator spriteAnimator;
    public EnemyHealth enemyHealth;

    [Header("Vision")]
    public float detectionDistance = 25f;
    public float visionAngle = 120f;
    public float aggroMemoryTime = 5f;
    public LayerMask obstacleMask;

    [Header("Ranges")]
    public float chaseDistance = 15f;
    public float attackDistance = 12f;
    public int damage = 25;

    [Header("Attack")]
    public float attackCooldown = 2f;

    [Header("Movement")]
    public float strafeRadius = 3f;
    public float strafeInterval = 2f;

    private float nextAttackTime = 0f;
    private float lastSeenTime;
    private NavMeshAgent agent;
    private bool hasAggro = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip attack;
    public float volume = 1f;


    void Awake()
    {
        if (player == null)
            player = Camera.main.transform;
        if (enemyHealth == null)
                    enemyHealth = GetComponent<EnemyHealth>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (enemyHealth.getCurHp() <= 0)
        {
            Destroy(agent);
            Destroy(this);
        }
        float distance = Vector3.Distance(transform.position, player.position);

        // Check if player is visible
        if (CanSeePlayer(distance))
        {
            hasAggro = true;
            lastSeenTime = Time.time;
        }

        if (hasAggro && Time.time - lastSeenTime > aggroMemoryTime)
            hasAggro = false;

        if (!hasAggro)
        {
            animator.SetState(EnemyState.Idle);
            agent.isStopped = true;
            return;
        }

        HandleCombat(distance);
    }

    bool CanSeePlayer(float distance)
    {
        if (distance < 3f)
            return true;

        if (distance > detectionDistance) return false;

        Vector3 origin = firePoint != null ? firePoint.position : transform.position + Vector3.up;
        Vector3 dir = (player.position - origin).normalized;

        // Vision cone
        float angle = Vector3.Angle(transform.forward, dir);
        if (angle > visionAngle * 0.5f)
            return false;

        // Raycast ONLY against player + obstacles
        int mask = LayerMask.GetMask("Default", "player"); 

        if (Physics.Raycast(origin, dir, out RaycastHit hit, detectionDistance, mask))
        {
            Debug.DrawLine(origin, hit.point, Color.red); // blocked
            if (hit.transform == player)
            {
                return true;
            }
        }

        Debug.DrawRay(origin, dir * detectionDistance, Color.yellow); // no hit
        return false;
    }

    void HandleCombat(float distance)
    {
        // Face player
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0;
        if (lookDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDir);

        // ATTACK
        if (distance <= attackDistance && Time.time >= nextAttackTime)
        {
            agent.isStopped = true;
            animator.SetState(EnemyState.Attack);
            audioSource.PlayOneShot(attack, volume);
            DoRaycastAttack();
            return;
        }

        // CHASE
        if (distance <= chaseDistance)
        {
            animator.SetState(EnemyState.Walk);
            StrafeAroundPlayer();
            return;
        }

        // IDLE
        agent.isStopped = true;
        animator.SetState(EnemyState.Idle);
    }

    void StrafeAroundPlayer()
    {
        agent.isStopped = false;

        Vector3 right = Vector3.Cross(Vector3.up, (player.position - transform.position).normalized);
        Vector3 target = player.position + right * Mathf.Sin(Time.time * strafeInterval) * strafeRadius;

        agent.SetDestination(target);
    }

    void DoRaycastAttack()
    {
        animator.PlayAttack();
        nextAttackTime = Time.time + attackCooldown;

        // Trigger animation frame for attack (optional)
        spriteAnimator.triggerFrame = 2;
        spriteAnimator.onTriggerFrame = () =>
        {
            RaycastHit hit;
            Vector3 dir = (player.position - firePoint.position).normalized;

            if (Physics.Raycast(firePoint.position, dir, out hit, attackDistance))
            {
                Debug.DrawLine(firePoint.position, hit.point, Color.red, 1f);

                PlayerHealth health = hit.transform.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
            }

            // Clear trigger so it only fires once
            spriteAnimator.onTriggerFrame = null;
        };
    }
}