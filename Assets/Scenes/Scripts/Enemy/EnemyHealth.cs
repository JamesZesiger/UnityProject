using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;


    [Header("Animation")]
    public EnemyAnimator animator;

    void Awake()
    {
        currentHealth = maxHealth;

        if (animator == null)
            animator = GetComponent<EnemyAnimator>();
    }

    public void TakeDamage(int amount, Vector3 hitPosition)
    {
        currentHealth -= amount;
        Debug.Log($"{name} took {amount} damage! ({currentHealth}/{maxHealth})");

        // Direction relative to enemy
        Vector3 localHitDir = transform.InverseTransformDirection(
            (hitPosition - transform.position).normalized
        );

        if (animator != null)
            animator.PlayHit(localHitDir);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{name} died!");

        if (animator != null)
        {
            animator.PlayDeath();
        }

        // Disable AI & collider so it stops interacting
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) agent.isStopped = true;

        var col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        Destroy(this);
    }

    public int getCurHp()
    {
        return currentHealth;
    }
}