/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 
using TMPro;

public class QuestGiver : MonoBehaviour {
    
    public float distance;
    public GameObject ActionDisplay;
    public GameObject ActionText;
    public GameObject player;
    public GameObject TextBox;
    public GameObject NPCName;
    public GameObject NPCText;

    void Update()
    {
        distance = PlayerCasting.distanceFromTarget;   
    }

    void OnMouseOver()
    { 
        //if(distance <= 3)
        if(distance <= 15)
        {
            //AttackBlocker.BlockSword = 1;
            //ActionText.GetComponent<Text>().text = "Talk";
            ActionText.GetComponent<TMP_Text>().text = "Talk";

            ActionDisplay.SetActive(true);
            ActionText.SetActive(true);            
        }
        if(Input.GetButtonDown("Action"))
        { Debug.Log("In Action");
            if(distance <= 15)
            {
                //AttackBlocker.BlockSword = 2;
                Screen.lockCursor = false;
                Cursor.visible = true;
                ActionDisplay.SetActive(false);
                ActionText.SetActive(false);
                StartCoroutine(NPCActive());
            }
        }
    }

    void OnMouseExit()
    {
        //AttackBlocker.BlockSword = 0;
        ActionDisplay.SetActive(false);
        ActionText.SetActive(false);
    }

    IEnumerator NPCActive()
    {
        TextBox.SetActive(true);
        //NPCName.GetComponent<Text>().text = "Warrior";
        NPCName.GetComponent<TMP_Text>().text = "Warrior";
        NPCName.SetActive(true);
        //NPCText.GetComponent<Text>().text = "Hello there! I have a quest for you. Please help me to find my treasur!";
        NPCText.GetComponent<TMP_Text>().text = "Hello there! I have a quest for you. Please help me to find my treasur!";
        NPCText.SetActive(true);       
        yield return new WaitForSeconds(5.0f);
        NPCName.SetActive(false);
        NPCText.SetActive(false);
        TextBox.SetActive(false);
        ActionDisplay.SetActive(false);
        ActionText.SetActive(false);
    }
} */


using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour
{
    public float interactionDistance = 15f;
    public GameObject ActionDisplay;
    public GameObject ActionText;
    public GameObject player;

    // Panneaux de dialogue séparés
    public GameObject NPCBox;
    public GameObject PlayerBox;

    // UI pour le PNJ
    public GameObject NPCName;
    public GameObject NPCText;

    // UI pour le Joueur
    public GameObject PlayerName;
    public GameObject PlayerText;

    // Panneau des choix et boutons
    public GameObject ChoicesPanel;
    public Button Quest1Button;
    public Button Quest2Button;
    public Button Quest3Button;

    private bool isTalking = false;

    void Start()
    {
        ChoicesPanel.SetActive(false);
        NPCBox.SetActive(false);
        PlayerBox.SetActive(false);

        if (Quest1Button != null && Quest2Button != null && Quest3Button != null)
        {
            Quest1Button.onClick.AddListener(() => SelectQuest("Finding a treasure"));
            Quest2Button.onClick.AddListener(() => SelectQuest("Saving a village"));
            Quest3Button.onClick.AddListener(() => SelectQuest("Hunting vampires"));

            Quest1Button.GetComponentInChildren<TMP_Text>().text = "Finding a treasure";
            Quest2Button.GetComponentInChildren<TMP_Text>().text = "Saving a village";
            Quest3Button.GetComponentInChildren<TMP_Text>().text = "Hunting vampires";
        }
        else
        {
            Debug.LogError("Les boutons ne sont pas assignés dans l'Inspector !");
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (Input.GetButtonDown("Action") && distance <= interactionDistance && !isTalking)
        {
            StartConversation();
        }
    }

    void OnMouseOver()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= interactionDistance && !isTalking)
        {
            ActionText.GetComponent<TMP_Text>().text = "Talk";
            ActionDisplay.SetActive(true);
            ActionText.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        if (!isTalking)
        {
            ActionDisplay.SetActive(false);
            ActionText.SetActive(false);
        }
    }

    void StartConversation()
    {
        isTalking = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ActionDisplay.SetActive(false);
        ActionText.SetActive(false);

        // Affichage du texte du joueur dans son propre panneau
        PlayerBox.SetActive(true);
        PlayerName.GetComponent<TMP_Text>().text = "Player";
        PlayerName.SetActive(true);
        PlayerText.GetComponent<TMP_Text>().text = "Hello, do you have a quest for me?";
        PlayerText.SetActive(true);

        StartCoroutine(WaitAndShowNPCResponse());
    }

    IEnumerator WaitAndShowNPCResponse()
    {
        yield return new WaitForSeconds(2.0f);

        // Cacher le panneau du joueur
        PlayerBox.SetActive(false);

        // Affichage du texte du PNJ dans son propre panneau
        NPCBox.SetActive(true);
        NPCName.GetComponent<TMP_Text>().text = "QG";
        NPCName.SetActive(true);
        NPCText.GetComponent<TMP_Text>().text = "Hello! I have some quests for you. \nChoose one:";
        NPCText.SetActive(true);

        ChoicesPanel.SetActive(true);
    }

    void SelectQuest(string questName)
    {
        ChoicesPanel.SetActive(false);
        NPCText.GetComponent<TMP_Text>().text = "See you, I wish you good luck \non your quest: " + questName + "!";

        StartCoroutine(CloseDialogue());
    }

    IEnumerator CloseDialogue()
    {
        yield return new WaitForSeconds(3.0f);

        NPCBox.SetActive(false);
        NPCName.SetActive(false);
        NPCText.SetActive(false);

        isTalking = false;

        // Vérifie si le joueur est en mode UI avant de cacher la souris
        if (!ChoicesPanel.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

}
