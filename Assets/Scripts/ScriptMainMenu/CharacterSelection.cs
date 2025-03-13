using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public GameObject characterPrefab;  
    public Transform previewSpawnPoint;       
    public TMP_Text characterNameText; 

    private GameObject currentPreviewInstance; 

    public void SpawnCharacter()
    {
        if (currentPreviewInstance != null)
        {
            Destroy(currentPreviewInstance);
        }

        currentPreviewInstance = Instantiate(characterPrefab, previewSpawnPoint.position, Quaternion.identity);
        
        currentPreviewInstance.transform.localScale = Vector3.one * 100f;
        currentPreviewInstance.transform.rotation = Quaternion.Euler(0, 180, 0);
        characterNameText.text = "Human Male"; 

    }

    public void EcranAccueil()
    {
        Debug.Log("Retour à l'écran d'accueil");
        SceneManager.LoadScene("SceneMainMenu");
    }
}
