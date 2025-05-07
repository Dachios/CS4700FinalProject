using UnityEngine;

public class HealthComponent : MonoBehaviour
{

    public int health = 100;

    public void TakeDamage(int amount) {
        health -= amount;
        if(health <= 0) {
            Die();
        }
    }

    void Die() {
        Debug.Log("Enemy Died");
        Destroy(gameObject);
    }
}
