using UnityEngine;

public class HitscanGun : Gun
{
    public Camera cam;
    public float range = 100f;

    void Awake()
    {
        cam = Camera.main;
    }
    protected override void Fire()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        Vector3 hitPoint = ray.origin + ray.direction * range;

        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            hitPoint = hit.point;
            // Apply damage here if needed
        }

    }
}