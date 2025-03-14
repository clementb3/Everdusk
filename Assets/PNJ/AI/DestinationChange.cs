using UnityEngine;
using UnityEngine.AI;

public class DestinationChange : MonoBehaviour
{
    public int xPos;
    public int zPos;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            // Générer une nouvelle position aléatoire
            xPos = Random.Range(400, 600);
            zPos = Random.Range(50, 280);

            // Déplacer l'objet de destination
            transform.position = new Vector3(xPos, 1.5f, zPos);

            // Mettre à jour la destination du NavMeshAgent
            NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.SetDestination(transform.position);
            }

            Debug.Log($"Nouvelle destination : {xPos}, 1.5, {zPos}");
        }
    }
}
