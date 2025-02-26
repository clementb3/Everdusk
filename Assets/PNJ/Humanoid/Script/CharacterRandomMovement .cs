using UnityEngine;
using UnityEngine.AI;

public class CharacterRandomMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public float minX = 300f;
    public float maxX = 600f;
    public float minZ = 300f;
    public float maxZ = 600f;
    public float stopDistance = 1f;
    public float detectionDistance = 30f;
    public float rotationSpeed = 5f;
    public Animator animator;

    private bool isNearPlayer = false;

    void Start()
    {
        MoveToRandomPosition();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionDistance)
        {
            // Arrêter le PNJ et regarder le joueur
            isNearPlayer = true;
            agent.isStopped = true; 
            animator.SetBool("isWalking", false); // Stopper l'animation de marche
            LookAtPlayer();
        }
        else
        {
            if (isNearPlayer)
            {
                isNearPlayer = false;
                agent.isStopped = false;
                animator.SetBool("isWalking", true); // Reprendre la marche
                MoveToRandomPosition();
            }

            if (!agent.pathPending && agent.remainingDistance <= stopDistance)
            {
                MoveToRandomPosition();
            }
        }
    }

    void MoveToRandomPosition()
    {
        Vector3 randomDestination = new Vector3(Random.Range(minX, maxX), 1.5f, Random.Range(minZ, maxZ));

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDestination, out hit, 5.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            animator.SetBool("isWalking", true); // Lancer l'animation de marche
        }
    }

    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; 

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
}
