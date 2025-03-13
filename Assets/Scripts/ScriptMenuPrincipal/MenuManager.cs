using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{
    public GameObject menuPrincipal; // Le panneau du menu principal
    private bool isMenuActive = false; // État du menu
    public MonoBehaviour playerController; // Le script qui contrôle le personnage

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        isMenuActive = !isMenuActive;
        menuPrincipal.SetActive(isMenuActive);

        if (playerController != null)
        {
            playerController.enabled = !isMenuActive; 
        }
    }


    public void EcranAccueil()
    {
        Debug.Log("Retour à l'écran d'accueil");
        SceneManager.LoadScene("SceneMainMenu");
    }
}
