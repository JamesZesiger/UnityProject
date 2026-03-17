using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;

    [Header("Visuals")]
    public DoomEnemySprite sprite; // reference to your sprite handler

    void Awake()
    {
        currentHealth = maxHealth;
        if (sprite == null)
            sprite = GetComponentInChildren<DoomEnemySprite>();
    }

    public void TakeDamage(int amount, Vector3 hitPosition)
    {
        currentHealth -= amount;
        Debug.Log($"{name} took {amount} damage! ({currentHealth}/{maxHealth})");

        // Calculate direction relative to enemy forward
        Vector3 localHitDir = transform.InverseTransformPoint(hitPosition).normalized;
        sprite.ShowHitDirection(localHitDir);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{name} died!");
        Destroy(gameObject); // or play death animation
    }
}