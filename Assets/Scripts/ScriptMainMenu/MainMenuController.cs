using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    //public GameObject selectionCanvas;

    public void GoToCharacterCreation()
    {
        SceneManager.LoadScene("SceneCreationPersonnage");
    }

    
    public void ShowCharacterSelection()
    {
        //selectionCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
