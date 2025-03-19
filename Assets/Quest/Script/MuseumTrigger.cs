using UnityEngine;

public class MuseumTrigger : MonoBehaviour
{   
    [SerializeField] private TreasureInteraction treasureScript; // Référence au script du coffre

    private void Start()
    {   
        // Vérifier si le script du coffre est assigné
        if (treasureScript == null)
        {
            Debug.LogError("TreasureInteraction non assigné ! Assignez-le dans l'Inspector.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            treasureScript?.SetNearMuseum(true); // ?. pour empêcher les NullReferenceException
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Le joueur est sorti du musée !");
            treasureScript?.SetNearMuseum(false);
        }
    }
}
