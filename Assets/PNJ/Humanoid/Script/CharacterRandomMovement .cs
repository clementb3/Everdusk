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

    public bool isNearPlayer = false;

    void Start()
    {
        MoveToRandomPosition();
    }

    void Update()
{
    float distanceToPlayer = Vector3.Distance(transform.position, player.position);

    if (distanceToPlayer <= detectionDistance)
    {
        isNearPlayer = true;
        agent.isStopped = true;
        animator.SetBool("isWalking", false);
        LookAtPlayer();
    }
    else
    {
        if (isNearPlayer)
        {
            isNearPlayer = false;
            agent.isStopped = false;
            animator.SetBool("isWalking", true); // ✅ S'assurer que l'animation reprend immédiatement
            MoveToRandomPosition();
        }

        if (!agent.pathPending && agent.remainingDistance <= stopDistance)
        {
            animator.SetBool("isWalking", false);
            MoveToRandomPosition();
        }
    }

    // ✅ Nouvelle vérification : s'assurer que l'animation suit bien le déplacement
    if (agent.velocity.magnitude > 0.1f) // Vérifie si l'agent bouge
    {
        animator.SetBool("isWalking", true);
    }
    else
    {
        animator.SetBool("isWalking", false);
    }
}


    void MoveToRandomPosition()
    {
        Vector3 randomDestination = new Vector3(Random.Range(minX, maxX), 1.5f, Random.Range(minZ, maxZ));

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDestination, out hit, 5.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            animator.SetBool("isWalking", true); // ✅ Lancer l'animation de marche
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
