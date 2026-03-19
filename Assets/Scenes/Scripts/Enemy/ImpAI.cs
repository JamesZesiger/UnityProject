using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ImpAI_NavMesh : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public EnemyAnimator animator;
    public Transform firePoint;
    public SpriteAnimator spriteAnimator;

    [Header("Vision")]
    public float detectionDistance = 20f;
    public float visionAngle = 120f;
    public float aggroMemoryTime = 5f;
    public LayerMask obstacleMask;

    [Header("Ranges")]
    public float chaseDistance = 12f;
    public float rangedDistance = 6f;
    public float meleeDistance = 2f;

    [Header("Attack")]
    public float attackCooldown = 2f;
    public int meleeDamage = 15;

    [Header("Ranged")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;

    [Header("Movement")]
    public float strafeRadius = 3f;
    public float strafeInterval = 2f;

    private float nextAttackTime = 0f;
    private float lastSeenTime;

    private NavMeshAgent agent;
    private bool hasAggro = false;

    void Awake()
    {
        if (player == null)
            player = Camera.main.transform;

        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (CanSeePlayer(distance))
        {
            hasAggro = true;
            lastSeenTime = Time.time;
        }

        if (hasAggro && Time.time - lastSeenTime > aggroMemoryTime)
        {
            hasAggro = false;
        }

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
        if (distance > detectionDistance) return false;

        Vector3 dir = (player.position - transform.position).normalized;

        float angle = Vector3.Angle(transform.forward, dir);
        if (angle > visionAngle / 2f) return false;

        if (Physics.Raycast(transform.position + Vector3.up, dir, out RaycastHit hit, detectionDistance, ~0))
        {
            if (hit.transform != player)
                return false;
        }

        return true;
    }

    void HandleCombat(float distance)
    {
        // Face the player
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0;
        if (lookDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDir);

        // MELEE first
        if (distance <= meleeDistance && Time.time >= nextAttackTime)
        {
            agent.isStopped = true;
            animator.SetState(EnemyState.Attack);
            DoMeleeAttack();
            return;
        }

        // RANGED
        if (distance <= rangedDistance && Time.time >= nextAttackTime)
        {
            agent.isStopped = true;
            animator.SetState(EnemyState.Attack);
            DoRangedAttack();
            return;
        }

        // CHASE
        if (distance <= chaseDistance)
        {
            animator.SetState(EnemyState.Walk);
            StrafeAroundPlayer();
            return;
        }

        // Idle
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

    // Immediately sets cooldown
void DoMeleeAttack()
{
    nextAttackTime = Time.time + attackCooldown;

    spriteAnimator.triggerFrame = 2;
    spriteAnimator.onTriggerFrame = () =>
    {
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        if (health != null)
            health.TakeDamage(meleeDamage);

        // ✅ Only trigger once
        spriteAnimator.onTriggerFrame = null;
    };
}

void DoRangedAttack()
{
    nextAttackTime = Time.time + attackCooldown;

    spriteAnimator.triggerFrame = 2;
    spriteAnimator.onTriggerFrame = () =>
    {
        ShootProjectile();

        // ✅ Important: clear the trigger so it only fires once
        spriteAnimator.onTriggerFrame = null;
    };
}

    void ShootProjectile()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject projObj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Vector3 dir = (player.position - firePoint.position).normalized;

        ImpProjectile proj = projObj.GetComponent<ImpProjectile>();
        if (proj != null)
        {
            proj.Launch(dir);
        }
    }
}