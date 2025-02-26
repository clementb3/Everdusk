/* // Good with low speed

using UnityEngine;
using UnityEngine.AI;

public class RandomDestination : MonoBehaviour
{
    public int xPos;
    public int zPos;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            // Générer une nouvelle position aléatoire
            xPos = Random.Range(200, 300);
            zPos = Random.Range(200, 300);

            // Déplacer l'objet de destination
            transform.position = new Vector3(xPos, 1.5f, zPos);

            // Mettre à jour la destination du NavMeshAgent
            NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.SetDestination(transform.position);
            }
        }
    }
}*/

using UnityEngine;
using UnityEngine.AI;

public class RandomDestination : MonoBehaviour
{
    public int xPos;
    public int zPos;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            // Générer une nouvelle position aléatoire
            xPos = Random.Range(300, 600);
            zPos = Random.Range(300, 600);

            // Déplacer l'objet de destination
            transform.position = new Vector3(xPos, 1.5f, zPos);

            // Mettre à jour la destination du NavMeshAgent
            NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                // Réinitialiser le chemin pour forcer le recalcul
                agent.ResetPath();  
                agent.SetDestination(transform.position);

                // Augmenter la vitesse de rotation et la réactivité
                agent.angularSpeed = 500f;
                agent.acceleration = 20f;
                agent.stoppingDistance = 1f;
                agent.autoBraking = true;
            }
        }
    }
}

