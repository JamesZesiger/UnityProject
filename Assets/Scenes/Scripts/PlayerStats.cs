using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int maxAmmo = 200;
    public int maxArmor = 100;

    public int health = 100;
    public int ammo = 50;
    public int armor = 0;

    public bool AddHealth(int amount)
    {
        if (health >= maxHealth) return false;

        health = Mathf.Min(health + amount, maxHealth);
        Debug.Log("Picked up health: " + amount);
        return true;
    }

    public bool AddAmmo(int amount)
    {
        if (ammo >= maxAmmo) return false;

        ammo = Mathf.Min(ammo + amount, maxAmmo);
        Debug.Log("Picked up ammo: " + amount);
        return true;
    }

    public bool AddArmor(int amount)
    {
        if (armor >= maxArmor) return false;

        armor = Mathf.Min(armor + amount, maxArmor);
        Debug.Log("Picked up armor: " + amount);
        return true;
    }
}