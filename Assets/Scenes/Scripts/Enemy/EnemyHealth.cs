using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;
    public AudioSource audioSource;
    public AudioClip hit;
    public AudioClip death;
    public float volume = 1f;
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
        audioSource.PlayOneShot(hit, volume);

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

        if (animator != null)
        {
            animator.PlayDeath();
        }
        audioSource.PlayOneShot(death, volume);
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