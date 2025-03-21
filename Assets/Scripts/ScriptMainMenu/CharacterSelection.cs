using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public GameObject characterPrefab;  
    public Transform previewSpawnPoint;       
    public TMP_Text characterNameText; 
    public static string selectedCharacter = null; 
    public Button confirmButton;
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
    

    void Start()
    {
        confirmButton.interactable = false; // Désactive le bouton au départ
    }

    public void SelectCharacter(string characterName)
    {
        selectedCharacter = characterName;
        confirmButton.interactable = true; // Active le bouton quand un personnage est choisi
    }

    public void ConfirmSelection()
    {
        if (!string.IsNullOrEmpty(selectedCharacter))
        {
            PlayerPrefs.SetString("SelectedCharacter", selectedCharacter); // Sauvegarde le choix
            SceneManager.LoadScene("main"); // Charge la scène du jeu
        }
    }

}
