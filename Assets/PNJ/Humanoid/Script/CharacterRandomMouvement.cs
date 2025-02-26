/*using UnityEngine;
using UnityEngine.AI;

public class CharacterRandomMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public float minX = 300f;
    public float maxX = 600f;
    public float minZ = 300f;
    public float maxZ = 600f;
    public float stopDistance = 1f; // Distance pour considérer la destination atteinte

    void Start()
    {
        MoveToRandomPosition();
    }

    void Update()
    {
        // Vérifier si l'agent est arrivé à destination
        if (!agent.pathPending && agent.remainingDistance <= stopDistance)
        {
            MoveToRandomPosition();
        }
    }

    void MoveToRandomPosition()
    {
        Vector3 randomDestination = new Vector3(Random.Range(minX, maxX), 1.5f, Random.Range(minZ, maxZ));

        // Vérifier que la position est navigable
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDestination, out hit, 5.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}*/

 // Pour ajuster la destination si trop proche
using UnityEngine;
using UnityEngine.AI;

public class CharacterRandomMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public float minX = 300f;
    public float maxX = 600f;
    public float minZ = 300f;
    public float maxZ = 600f;
    public float stopDistance = 1f; // Distance pour considérer la destination atteinte
    public float adjustDistance = 20f; // ✅ Distance supplémentaire si trop proche

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = agent.transform.position;
        MoveToRandomPosition();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= stopDistance)
        {
            MoveToRandomPosition();
        }
    }

    void MoveToRandomPosition()
    {
        Vector3 randomDestination = new Vector3(Random.Range(minX, maxX), 1.5f, Random.Range(minZ, maxZ));
        float maxPossibleDistance = Mathf.Sqrt(Mathf.Pow(maxX - minX, 2) + Mathf.Pow(maxZ - minZ, 2));
        float minAllowedDistance = maxPossibleDistance / 4; // ✅ 1/4 de la distance max

        // Vérifier si la destination est trop proche
        float distance = Vector3.Distance(lastPosition, randomDestination);
        if (distance < minAllowedDistance)
        {
            Vector3 direction = (randomDestination - lastPosition).normalized;
            randomDestination += direction * adjustDistance; // ✅ Ajoute une distance fixe dans la même direction
        }

        // Vérifier que la position est sur le NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDestination, out hit, 5.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            lastPosition = hit.position;
            Debug.Log($"Nouvelle destination ajustée : {hit.position}");
        }
    }
}

