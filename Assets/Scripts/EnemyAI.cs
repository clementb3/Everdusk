using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private Transform player;          // Reference to the player.
    private PlayerStat playerStat;     // Reference to the player stat class.

    [Header("ENEMY REFERENCES")]
    [SerializeField]
    private NavMeshAgent agent;        // Reference to the ennemy.
    [SerializeField]
    private Animator animator;         // Reference to the ennemys' animator.

    [Header("ENNEMY STATS")]
    [SerializeField]
    private float walkSpeed;           // Walk speed of enemy.
    [SerializeField]
    private float runSpeed;            // Run speed of enemy.
    [SerializeField]
    private float attackDamage;        // Number of damage points the enemy inflicts.
    [SerializeField]
    private float rotationSpeed;       // Speed at which the enemy rotates.
    [SerializeField]
    private float maxHealth;           // Max health of the enemy.
    private float currentHealth;       // Current health of the enemy.
    private bool isDead;               // Boolean to know if an enemy is dead and to disable its functionalities.

    [Header("WAIT PARAMETERS")]
    [SerializeField]
    private float waitTimeMin;         // Min time the enemy waits before moving.
    [SerializeField]
    private float waitTimeMax;         // Max time the enemy waits before moving.
    [SerializeField]
    private float attackDelay;         // Time before attacking.

    [Header("DISTANCE PARAMETERS")]
    [SerializeField]
    private float distMin;             // Min distance of enemy movement.
    [SerializeField]
    private float distMax;             // Max distance of enemy movement.
    [SerializeField]
    private float detectionRadius;     // Distance of detection.
    [SerializeField]
    private float attackRadius;        // Distance of attack.

    private bool destination;          // Enemy has a new destination ?
    private bool isAttacking;          // Enemy is attacking ?

    void Awake()
    {
        Transform playerTmp = GameObject.FindGameObjectWithTag("Player").transform;
        player = playerTmp;
        playerStat = playerTmp.GetComponent<PlayerStat>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Check if the player is in the enemys' detection area.
        if (Vector3.Distance(player.position, transform.position) <= detectionRadius && !playerStat.IsDead())
        {
            // Chase the player.
            agent.speed = runSpeed;
            // Rotate the enemy to front the player.
            Quaternion rotation = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            if (!isAttacking)
            {
                // Check if the player is in the enemys' attack area.
                if (Vector3.Distance(player.position, transform.position) < attackRadius)
                    StartCoroutine(AttackPlayer());
                else
                    agent.SetDestination(player.position);
            }
        }
        else
        {
            agent.speed = walkSpeed;
            // Set a random destination for the enemy.
            if (agent.remainingDistance < 0.75f && !destination)
                StartCoroutine(GetNewDestination());
        }
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    // Function to define a new destination for the enemy.
    IEnumerator GetNewDestination()
    {
        destination = true;
        yield return new WaitForSeconds(Random.Range(waitTimeMin, waitTimeMax));
        Vector3 newDest = transform.position + Random.Range(distMin, distMax) * new Vector3(Random.Range(-1f, 1), 0f, Random.Range(-1f, 1)).normalized;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(newDest, out hit, distMax, NavMesh.AllAreas))
            agent.SetDestination(hit.position);
        destination = false;
    }

    // Logic behind the enemys' attack.
    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        agent.isStopped = true;
        // Start the attacks' animation.
        animator.SetTrigger("Attack");
        // Inflicts damage to the player.
        playerStat.TakeDamage(attackDamage);
        yield return new WaitForSeconds(attackDelay);
        if(agent.enabled)
            agent.isStopped = false;
        isAttacking = false;
    }

    // Function to inflict damage to the enemy.
    public void TakeDamage(float damage)
    {
        if(isDead)
            return;
        currentHealth -= damage;
        if(currentHealth <= 0)
            Die();
        else 
            animator.SetTrigger("GetHit");
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Dead");
        // Unactive enemys' movement.
        agent.enabled = false;
        // Unactive script on the enemy.
        enabled = false;
    }

    // Debug for the detection and attack areas, draw their respective sphere.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
