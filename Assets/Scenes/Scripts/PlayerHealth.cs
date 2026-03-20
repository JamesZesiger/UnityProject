using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public GameObject player;
    private PlayerController controller;
    public PlayerHUD playerHUD;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hit;
    public AudioClip die;
    public float volume = 1f;

    [Header("Damage Overlay")]
    public Image damageOverlay;
    public float flashDuration = 0.4f;
    public float flashAlpha = 0.4f;
    private bool canTakeDamage = true;
    void Awake()
    {
        currentHealth = maxHealth;
        controller = GetComponentInChildren<PlayerController>();
        playerHUD = GetComponentInChildren<PlayerHUD>();
        playerHUD.SetHealth(currentHealth);
    }

    // Update is called once per frame
    public void TakeDamage(int amount)
    {
        if (canTakeDamage)
        {
            
            currentHealth -= amount;
            audioSource.PlayOneShot(hit, volume);
            if (currentHealth <= 0)
            {
                Die();
            }
            else if (amount>0)
                StartCoroutine(DamageFlash());
            
            playerHUD.SetHealth(currentHealth);
            
        }
    }

    void Die()
    {
        canTakeDamage = false;
        audioSource.PlayOneShot(die, volume);
        if (controller != null)
            controller.enabled = false;

        Color color = damageOverlay.color;
        color.a = 0.6f;
        damageOverlay.color = color;

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


    private IEnumerator DamageFlash()
    {
        Color color = damageOverlay.color;
        color.a = flashAlpha;
        damageOverlay.color = color;

        yield return new WaitForSeconds(flashDuration);

        color.a = 0f;
        damageOverlay.color = color;
    }
    public int getCurHp(){ return currentHealth;}
}
