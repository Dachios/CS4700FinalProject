using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{

    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    
    public int damage = 10;
    public float speed = 14.0f;

    private Transform player;
    private NavMeshAgent agent;
    private bool isAttacking = false;

    float extraRotationSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if(distance <= detectionRange) {
            agent.SetDestination(player.position);
            ExtraRotation();

            if(distance <= attackRange && !isAttacking) {
                StartCoroutine(AttackPlayer());
            }
        }
        
    }

    IEnumerator AttackPlayer() {
        isAttacking = true;
        Debug.Log("Enemy Attacks");

        if(player.TryGetComponent(out PlayerInfo playerHealth)) {
            playerHealth.TakeDamage(damage, false);
        }

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    

    void ExtraRotation()
    {
        Vector3 lookRotation = agent.steeringTarget-transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookRotation), extraRotationSpeed*Time.deltaTime);
    }
}
