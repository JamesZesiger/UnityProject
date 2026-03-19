using UnityEngine;
using System.Collections;

public class Rifle : HitscanGun
{
    [Header("Visuals")]
    public Transform muzzle; // assign via prefab
    public GameObject bulletTrailPrefab; // assign prefab

    [Header("Reload Animation")]
    public float reloadAngle = -45f;      // rotate gun by 45 degrees
    public float reloadDuration = 3f;    // how long reload lasts
    private Quaternion _initialRotation;
    public bool isReloading = false;

    public AudioClip shot;
    public AudioSource audioSource;
    void Awake()
    {
        base.Awake();
        _initialRotation = transform.localRotation;
        if (cam == null)
            cam = Camera.main;
    }

    protected override void Fire()
    {
        if (isReloading) return;
        audioSource.PlayOneShot(shot, 1.0f);
        Vector3 dir = cam.transform.forward;
        dir.Normalize();

        Vector3 hitPoint = cam.transform.position + dir * range;

        if (Physics.Raycast(cam.transform.position, dir, out RaycastHit hit, range))
        {
            hitPoint = hit.point;

            Debug.Log("Rife hit " + hit.collider.name);
            EnemyHealth health = hit.collider.GetComponentInParent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damage, hitPoint);
            }
        }

        // Spawn trail from muzzle to hit point
        if (bulletTrailPrefab != null && muzzle != null)
        {
            GameObject trail = Instantiate(bulletTrailPrefab, muzzle.position, Quaternion.identity);
            StartCoroutine(AnimateTrail(trail, muzzle.position, hitPoint));
        }
        
    }

    private System.Collections.IEnumerator AnimateTrail(GameObject trailObj, Vector3 start, Vector3 end)
    {
        float duration = 0.25f;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            if (trailObj != null)
                trailObj.transform.position = Vector3.Lerp(start, end, t / duration);
            yield return null;
        }
        if (trailObj != null) Destroy(trailObj);
    }


    public override void Reload()
    {
        // Start the reload animation coroutine
        StartCoroutine(ReloadAnimation());
    }

    private IEnumerator ReloadAnimation()
    {
        isReloading = true; // prevent firing during reload

        Quaternion sideRotation = _initialRotation * Quaternion.Euler(0f, 0f, reloadAngle);

        float rotateSpeed = 10f; // larger = faster rotation
        float holdTime = reloadDuration * 0.7f; // 70% of duration hold
        float snapTime = 0.1f; // time to snap back


        while (Quaternion.Angle(transform.localRotation, sideRotation) > 0.1f)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, sideRotation, rotateSpeed * 360f * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(holdTime);

        float elapsed = 0f;
        while (elapsed < snapTime)
        {
            transform.localRotation = Quaternion.Slerp(sideRotation, _initialRotation, elapsed / snapTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = _initialRotation;
        currentAmmo = magazineSize;

        isReloading = false;
    }
    public override void TryFire()
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
}