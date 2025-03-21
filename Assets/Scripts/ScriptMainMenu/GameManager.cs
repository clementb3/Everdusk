using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform spawnPoint; // Drag & Drop le point où le personnage doit apparaître
    public GameObject[] characterPrefabs; // Met tous tes personnages ici

    void Start()
    {
        string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter", "");

        if (!string.IsNullOrEmpty(selectedCharacter))
        {
            GameObject characterPrefab = FindCharacterPrefab(selectedCharacter);
            if (characterPrefab != null)
            {
                GameObject player = Instantiate(characterPrefab, spawnPoint.position, Quaternion.identity);
                player.transform.localScale = new Vector3(100, 100, 100); // Ajuste la taille
            }
            else
            {
                Debug.LogError("Aucun prefab trouvé pour " + selectedCharacter);
            }
        }
        else
        {
            Debug.LogWarning("Aucun personnage sélectionné !");
        }
    }

    GameObject FindCharacterPrefab(string name)
    {
        foreach (GameObject prefab in characterPrefabs)
        {
            if (prefab.name == name)
            {
                return prefab;
            }
        }
        return null;
    }
}
