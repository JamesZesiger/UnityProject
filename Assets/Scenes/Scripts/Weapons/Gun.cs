using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [Header("Gun Stats")]
    public float fireRate;
    public int damage;
    public int magazineSize = 8;
    public float reloadTime;

    [Header("Recoil")]
    public Vector3 recoilOffset = new Vector3(0f, 0f, -0.1f); // move back along local Z
    public float recoilReturnSpeed = 5f; // speed at which weapon moves back to original position

    private Vector3 _initialLocalPos;

    protected float nextFireTime;
    protected int currentAmmo;
    public int CurrentAmmo => currentAmmo;

    protected virtual void Awake()
    {
        _initialLocalPos = transform.localPosition; // store starting local position
        currentAmmo = magazineSize;
    }

    public virtual void TryFire()
    {
        if (Time.time < nextFireTime) return;
        if (currentAmmo <= 0) 
        {
            Reload();
            return;
        }

        Fire();
        
        transform.localPosition += recoilOffset;
        currentAmmo--;

        nextFireTime = Time.time + 1f / fireRate;
    }

    void Update()
    {
        // Smoothly move weapon back to initial position
        transform.localPosition = Vector3.Lerp(transform.localPosition, _initialLocalPos, Time.deltaTime * recoilReturnSpeed);
    }

    protected abstract void Fire();

    public virtual void Reload()
    {
        currentAmmo = magazineSize;
        nextFireTime = Time.time + 3f;
    }
}