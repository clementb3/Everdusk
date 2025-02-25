using System.Collections;
using Unity.VisualScripting;
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
    }

    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) <= detectionRadius && !playerStat.GetIsDead())
        {
            agent.speed = runSpeed;
            Quaternion rotation = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            if (!isAttacking)
            {
                if (Vector3.Distance(player.position, transform.position) < attackRadius)
                    StartCoroutine(AttackPlayer());
                else
                    agent.SetDestination(player.position);
            }
        }
        else
        {
            agent.speed = walkSpeed;
            if (agent.remainingDistance < 0.75f && !destination)
                StartCoroutine(GetNewDestination());
        }
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

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

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        agent.isStopped = true;
        animator.SetTrigger("Attack");
        playerStat.TakeDamage(attackDamage);
        yield return new WaitForSeconds(attackDelay);
        agent.isStopped = false;
        isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
