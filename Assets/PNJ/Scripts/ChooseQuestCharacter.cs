using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChooseQuestCharacter : MonoBehaviour
{
    public float interactionDistance = 10f;
    public GameObject ActionDisplay;
    public GameObject ActionText;
    public GameObject player;

    // UI elements
    public GameObject NPCBox;
    public GameObject PlayerBox;
    public GameObject NPCName;
    public GameObject NPCText;
    public GameObject PlayerName;
    public GameObject PlayerText;
    public GameObject ChoicesPanel;
    public Button Quest1Button;
    public Button Quest2Button;
    public Button Quest3Button;
    public Animator animator;

    private bool isTalking = false;
    private bool hasTalked = false;
    private bool waitingForPlayerResponse = false; // Ajout pour attendre la touche "E"

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

        // Si le joueur entre dans la zone d'interaction, démarrer la conversation si elle n'est pas en cours
        if (distance <= interactionDistance && !isTalking && !hasTalked)
        {
            StartConversation();
        }
        // Fin de la conversation si le joueur s'éloigne
        else if (distance > interactionDistance && hasTalked)
        {
            StartCoroutine(EndConversation());
        }

        // Le joueur appuie sur "E" pour répondre
        if (waitingForPlayerResponse && Input.GetKeyDown(KeyCode.E))
        {
            PlayerRespond();
        }
    }

    void StartConversation()
    {
        isTalking = true;
        hasTalked = true;
        waitingForPlayerResponse = false;

        // Réinitialisation de l'état de l'animator pour bien jouer l'animation de "parler"
        animator.SetBool("isTalking", true);

        Debug.Log("Starting conversation...");

        // Désactivation de l'affichage des actions
        ActionDisplay.SetActive(false);
        ActionText.SetActive(false);

        // Réinitialisation des éléments de texte du PNJ avant d'afficher le texte
        NPCBox.SetActive(true);
        NPCName.GetComponent<TMP_Text>().text = "NPC";
        NPCText.GetComponent<TMP_Text>().text = ""; // Réinitialiser le texte à chaque nouvelle rencontre
        Debug.Log("NPCText reset!");

        // Affichage du texte du PNJ
        StartCoroutine(DisplayNPCText("Hello, can I help you?"));
    }

    IEnumerator DisplayNPCText(string text)
    {
        Debug.Log("Displaying NPC text: " + text);
        NPCText.GetComponent<TMP_Text>().text = text; // Affichage du texte
        yield return new WaitForSeconds(1.0f); // Attendre un peu avant de montrer la réponse du joueur

        // Afficher "Press E to respond"
        ActionText.GetComponent<TMP_Text>().text = "E";
        ActionDisplay.SetActive(true);
        ActionText.SetActive(true);
        waitingForPlayerResponse = true; // Active l'attente de réponse
    }

    void PlayerRespond()
    {
        waitingForPlayerResponse = false;
        ActionDisplay.SetActive(false);
        ActionText.SetActive(false);

        // Afficher la réponse du joueur
        PlayerBox.SetActive(true);
        PlayerName.GetComponent<TMP_Text>().text = "Player";
        PlayerText.GetComponent<TMP_Text>().text = "Yes, do you have a quest for me?";

        // Passer à l'affichage des choix de quête
        StartCoroutine(ShowQuestOptions());
    }

    IEnumerator ShowQuestOptions()
    {
        yield return new WaitForSeconds(2.0f);

        PlayerBox.SetActive(false);
        NPCBox.SetActive(true);
        NPCName.GetComponent<TMP_Text>().text = "NPC";
        NPCText.GetComponent<TMP_Text>().text = "Yes! Here are some quests:";

        ChoicesPanel.SetActive(true);
    }

    void SelectQuest(string questName)
    {
        ChoicesPanel.SetActive(false);
        NPCText.GetComponent<TMP_Text>().text = "Good luck with your quest: \n  " + questName + "!";

        StartCoroutine(CloseDialogue());
    }

    IEnumerator CloseDialogue()
    {
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(EndConversation());
    }
    
    //void EndConversation()
    IEnumerator EndConversation()
    {
        Debug.Log("Ending conversation...");
        NPCBox.SetActive(false);
        isTalking = false;
        animator.SetBool("isTalking", false); 

        // Réinitialiser l'état de la conversation
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        animator.SetBool("isWalking", true);

        yield return new WaitForSeconds(3.0f);

        hasTalked = false; // Permet au PNJ de parler à nouveau quand le joueur revient
    }

}