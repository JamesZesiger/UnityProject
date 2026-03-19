using UnityEngine;

public class ImpProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public float lifetime = 5f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Call this to launch the projectile
    public void Launch(Vector3 direction)
    {
        if (rb != null)
        {
            rb.linearVelocity = direction.normalized * speed;
        }

        transform.forward = direction; // optional: rotate sprite/model
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("ball colide");
        if (other.CompareTag("Player"))
        {
            Debug.Log("ball colide with player");
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
                ph.TakeDamage(damage);
                Debug.Log("damage");
        }

        Destroy(gameObject); // destroy on impact
    }
}