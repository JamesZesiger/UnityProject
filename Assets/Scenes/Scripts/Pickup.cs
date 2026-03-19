using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PickupType
    {
        Ammo,
        Health,
        Armor
    }

    public PickupType type;
    public int amount = 10;

    public AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerStats player = other.GetComponent<PlayerStats>();
        if (player == null) return;

        bool pickedUp = false;

        switch (type)
        {
            case PickupType.Ammo:
                pickedUp = player.AddAmmo(amount);
                break;

            case PickupType.Health:
                pickedUp = player.AddHealth(amount);
                break;

            case PickupType.Armor:
                pickedUp = player.AddArmor(amount);
                break;
        }

        if (pickedUp)
        {
            if (pickupSound)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            Destroy(gameObject);
        }
    }
}