using UnityEngine;
public class ProjectileGun : Gun
{
    public GameObject projectilePrefab;
    public Transform muzzle;

    protected override void Fire()
    {
        Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
    }
}