using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleProjectile : MonoBehaviour
{
    public int damage = 10;
    public float speed = 25f;
    public float life = 4f;

    void OnEnable()
    {
        var rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;

        // ignore the collider on the object that fired us
        if (transform.parent != null)
        {
            Collider shooterCol = transform.parent.GetComponent<Collider>();
            Collider myCol      = GetComponent<Collider>();
            if (shooterCol && myCol)
                Physics.IgnoreCollision(myCol, shooterCol);
        }

        Destroy(gameObject, life);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerInfo>()?.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}

