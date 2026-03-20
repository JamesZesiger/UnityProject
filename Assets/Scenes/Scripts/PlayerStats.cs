using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public PlayerHealth hpComponent;
    public int maxHealth = 100;
    public int maxAmmo = 200;
    public int maxArmor = 100;

    public int health = 100;
    public int ammo = 50;
    public int armor = 0;

    public bool AddHealth(int amount)
    {
        if (hpComponent.getCurHp() >= maxHealth) return false;

        hpComponent.TakeDamage(-amount);
        return true;
    }

    public bool AddAmmo(int amount)
    {
        if (ammo >= maxAmmo) return false;

        ammo = Mathf.Min(ammo + amount, maxAmmo);
        return true;
    }

    public bool AddArmor(int amount)
    {
        if (armor >= maxArmor) return false;

        armor = Mathf.Min(armor + amount, maxArmor);
        return true;
    }
}