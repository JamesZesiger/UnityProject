using UnityEngine;
using System.Collections;
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public GameObject player;
    private PlayerController controller;

    private bool canTakeDamage = true;
    void Awake()
    {
        currentHealth = maxHealth;
        controller = GetComponentInChildren<PlayerController>();
    }

    // Update is called once per frame
    public void TakeDamage(int amount)
    {
        if (canTakeDamage)
        {
            currentHealth -= amount;

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        canTakeDamage = false;
        Debug.Log("Player died");

        if (controller != null)
            controller.enabled = false;

        StartCoroutine(FallOver());
    }

    private IEnumerator FallOver()
    {
        Quaternion startRot = player.transform.rotation;
        Quaternion targetRot = startRot * Quaternion.Euler(0f, 0f, 90f);

        float duration = 0.5f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            player.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        player.transform.rotation = targetRot;
    }
}
