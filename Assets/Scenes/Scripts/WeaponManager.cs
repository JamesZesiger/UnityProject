using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] Transform weaponHolder;
    
    [SerializeField] GameObject startingWeapon;
    Gun currentGun;
    public PlayerHUD playerHUD;


    public void Equip(GameObject gunPrefab)
    {
        if (currentGun != null)
            Destroy(currentGun.gameObject);

        GameObject gun = Instantiate(gunPrefab, weaponHolder);
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localRotation = Quaternion.identity;

        currentGun = gun.GetComponent<Gun>();
        if(currentGun == null)
        Debug.LogError("Gun component missing on prefab!");

        if(playerHUD != null)
            playerHUD.SetGun(currentGun); // update HUD to current weapon
    }

    public void Fire()
    {
        if (currentGun != null)
            currentGun.TryFire();
    }

    public void Reload()
    {
        if (currentGun != null)
            currentGun.Reload();
    }

    void Start()
    {
        Equip(startingWeapon);
    }
}