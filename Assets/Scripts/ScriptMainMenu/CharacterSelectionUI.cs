using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Importer TextMeshPro
using System.Collections.Generic;

public class CharacterSelectionUI : MonoBehaviour
{
    public GameObject characterButtonPrefab;  // Bouton pour chaque personnage
    public Transform gridContainer;          // Où placer les boutons
    public Image previewImage;               // Image du personnage sélectionné
    public TMP_Text previewText;             // Texte avec les infos du personnage
    public Button confirmButton;             // Bouton de confirmation

    [System.Serializable]
    public class CharacterData
    {
        public string name;
        public string characterClass;
        public Sprite appearance;
    }

    public List<CharacterData> availableCharacters = new List<CharacterData>();

    void Start()
    {
        confirmButton.interactable = false;
        GenerateCharacterButtons();
    }

    void GenerateCharacterButtons()
    {
        foreach (var character in availableCharacters)
        {
            GameObject buttonObj = Instantiate(characterButtonPrefab, gridContainer);
            Button button = buttonObj.GetComponent<Button>();
            TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();  // Utilisation de TMP_Text

            buttonText.text = character.name;
            button.onClick.AddListener(() => SelectCharacter(character));
        }
    }

    void SelectCharacter(CharacterData character)
    {
        previewImage.sprite = character.appearance;
        previewText.text = $"{character.name}\nClass: {character.characterClass}";
        confirmButton.interactable = true;
    }
}
