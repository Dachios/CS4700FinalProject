using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class SimpleProjectile : MonoBehaviour
{
    public int damage  = 10;
    public float speed = 25f;
    public float life  = 4f;
    public GameObject hitFx;          // spark / blood prefab (optional)

    Rigidbody rb;
    Collider  col;

    void Awake()
    {
        rb  = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        rb.useGravity   = false;      // bullet stays level
        col.isTrigger   = true;       // trigger so no push-back
    }

    // shooter calls this immediately after Instantiate
    public void Init(GameObject shooter)
    {
        foreach (Collider c in shooter.GetComponentsInChildren<Collider>())
            Physics.IgnoreCollision(col, c, true);    // ignore own shooter
    }

    void OnEnable()
    {
        rb.linearVelocity = transform.forward * speed;      // start moving
        Destroy(gameObject, life);                    // fail-safe cleanup
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var hp = other.GetComponent<PlayerInfo>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
                Debug.Log($"Projectile hit player – {damage} dmg applied");
            }
        }

        if (hitFx) Instantiate(hitFx, transform.position, Quaternion.identity);
        Destroy(gameObject);                          // vanish on first contact
    }
}


