using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class CharacterData
{
    public string name;
    public string characterClass;
}

public class CharacterManager : MonoBehaviour
{
    private List<CharacterData> characters = new List<CharacterData>();

    public void SaveCharacter(string characterName, string characterClass)
    {
        // Charger les personnages existants
        LoadCharacters();

        // Ajouter un nouveau personnage
        CharacterData newCharacter = new CharacterData { name = characterName, characterClass = characterClass };
        characters.Add(newCharacter);

        // Sauvegarde sous forme de JSON
        string json = JsonUtility.ToJson(new CharacterList { characters = characters });
        PlayerPrefs.SetString("SavedCharacters", json);
        PlayerPrefs.Save();
    }

    public List<CharacterData> LoadCharacters()
    {
        if (PlayerPrefs.HasKey("SavedCharacters"))
        {
            string json = PlayerPrefs.GetString("SavedCharacters");
            CharacterList data = JsonUtility.FromJson<CharacterList>(json);
            characters = data.characters;
        }
        return characters;
    }

    [System.Serializable]
    private class CharacterList
    {
        public List<CharacterData> characters;
    }
}
