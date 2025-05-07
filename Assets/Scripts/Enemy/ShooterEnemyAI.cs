using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ShooterEnemyAI : MonoBehaviour
{
    [Header("Distances")]
    public float minRange = 5f;   // retreat if closer
    public float maxRange = 10f;  // advance if farther
    public float restRange = 7f;  // idle band to stop chattering

    [Header("Firing")]
    public Transform muzzle;         // where bullets spawn
    public GameObject projectile;    // prefab to instantiate
    public float cadence = 1.2f;     // seconds between shots
    public float spreadDegrees = 3f; // random inaccuracy

    NavMeshAgent agent;
    Transform player;
    float nextFireTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        // movement decisions
        if (dist < minRange)
        {
            // step back
            Vector3 retreatDir = (transform.position - player.position).normalized;
            Vector3 retreatPos = transform.position + retreatDir * (minRange - dist);
            agent.SetDestination(retreatPos);
        }
        else if (dist > maxRange)
        {
            // step closer
            agent.SetDestination(player.position);
        }
        else if (dist < restRange && agent.hasPath)
        {
            // within idle band
            agent.ResetPath();
        }

        // face the player
        Vector3 lookPos = player.position;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);

        // reset cooldown if player sprints well beyond max range
        if (dist > maxRange + 3f) nextFireTime = 0f;

        // shoot when ready
        if (Time.time >= nextFireTime && dist <= maxRange)
        {
            Shoot();
            nextFireTime = Time.time + cadence;
        }
    }

    void Shoot()
    {
        if (projectile == null || muzzle == null) return;

        Vector3 dir = (player.position - muzzle.position).normalized;
        dir = Quaternion.Euler(
                Random.Range(-spreadDegrees, spreadDegrees),
                Random.Range(-spreadDegrees, spreadDegrees),
                0f) * dir;

        // 1. spawn the projectile
        GameObject clone = Instantiate(
            projectile,
            muzzle.position,
            Quaternion.LookRotation(dir)
        );

        // 2. detach (just in case)
        clone.transform.parent = null;

        // 3. tell it to ignore every collider on this enemy
        clone.GetComponent<SimpleProjectile>().Init(gameObject);
    }



}
