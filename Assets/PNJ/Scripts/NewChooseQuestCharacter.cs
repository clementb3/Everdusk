using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NewChooseQuestCharacter : MonoBehaviour
{
    public float interactionDistance = 2f;
    private GameObject player;

    private GameObject ActionDisplay;
    private GameObject ActionText;

    // UI elements
    private GameObject NPCBox;
    private GameObject PlayerBox;
    private GameObject NPCName;
    private GameObject NPCText;
    private GameObject PlayerName;
    private GameObject PlayerText;
    public GameObject ChoicesPanel;
    public Button Quest1Button;
    public Button Quest2Button;
    public Button Quest3Button;
    private Animator animator;

    private bool isTalking = false;
    private bool hasTalked = false;
    private bool waitingForPlayerResponse = false;

    void Start()
    {
        // Trouver dynamiquement les objets dans la scène
        player = GameObject.FindWithTag("Player");
        ActionDisplay = GameObject.Find("ActionDisplay");
        ActionText = GameObject.Find("ActionText");
        NPCBox = GameObject.Find("NPCBox");
        PlayerBox = GameObject.Find("PlayerBox");
        NPCName = GameObject.Find("NPCName");
        NPCText = GameObject.Find("NPCText");
        PlayerName = GameObject.Find("PlayerName");
        PlayerText = GameObject.Find("PlayerText");
        //ChoicesPanel = GameObject.Find("ChoicesPanel");

        // Trouver les boutons
        //Quest1Button = GameObject.Find("Quest1Button").GetComponent<Button>(); 
        //Quest2Button = GameObject.Find("Quest2Button").GetComponent<Button>();
        //Quest3Button = GameObject.Find("Quest3Button").GetComponent<Button>();

        // Vérification si tous les objets sont bien trouvés
        if (Quest1Button != null && Quest2Button != null && Quest3Button != null)
        {
            Quest1Button.onClick.AddListener(() => SelectQuest("1 -> Finding a treasure"));
            Quest2Button.onClick.AddListener(() => SelectQuest("2 -> Saving a village"));
            Quest3Button.onClick.AddListener(() => SelectQuest("3 -> Hunting vampires"));

            Quest1Button.GetComponentInChildren<TMP_Text>().text = "1 -> Finding a treasure";
            Quest2Button.GetComponentInChildren<TMP_Text>().text = "2 -> Saving a village";
            Quest3Button.GetComponentInChildren<TMP_Text>().text = "3 -> Hunting vampires";
        }
        else
        {
            Debug.LogError("Les boutons ne sont pas trouvés dans la scène !");
        }

        ChoicesPanel.SetActive(false);
        NPCBox.SetActive(false);
        PlayerBox.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // Si le joueur entre dans la zone d'interaction, démarrer la conversation si elle n'est pas en cours
        if (distance <= interactionDistance && !isTalking && !hasTalked)
        {
            StartConversation();
        }
        else if (distance > interactionDistance && hasTalked)
        {
            StartCoroutine(EndConversation());
        }

        // Le joueur appuie sur "E" pour répondre
        if (waitingForPlayerResponse && Input.GetKeyDown(KeyCode.E))
        {
            PlayerRespond();
        }

        // Ajout de la gestion des touches 1, 2 et 3 pour sélectionner les quêtes
        if (ChoicesPanel.activeSelf) // Si le panneau de choix est actif
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) // Si la touche "1" est pressée
            {
                SelectQuest("Finding a treasure");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) // Si la touche "2" est pressée
            {
                SelectQuest("Saving a village");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)) // Si la touche "3" est pressée
            {
                SelectQuest("Hunting vampires");
            }
        }
    }

    void StartConversation()
    {
        isTalking = true;
        hasTalked = true;
        waitingForPlayerResponse = false;

        animator.SetBool("isTalking", true);

        // Désactivation de l'affichage des actions
        ActionDisplay.SetActive(false);
        ActionText.SetActive(false);

        NPCBox.SetActive(true);
        NPCName.GetComponent<TMP_Text>().text = "NPC";
        NPCText.GetComponent<TMP_Text>().text = "";

        StartCoroutine(DisplayNPCText("Hello, can I help you?"));
    }

    IEnumerator DisplayNPCText(string text)
    {
        NPCText.GetComponent<TMP_Text>().text = text;
        yield return new WaitForSeconds(1.0f);

        ActionText.GetComponent<TMP_Text>().text = "E";
        ActionDisplay.SetActive(true);
        ActionText.SetActive(true);
        waitingForPlayerResponse = true;
    }

    void PlayerRespond()
    {
        waitingForPlayerResponse = false;
        ActionDisplay.SetActive(false);
        ActionText.SetActive(false);

        PlayerBox.SetActive(true);
        PlayerName.GetComponent<TMP_Text>().text = "Player";
        PlayerText.GetComponent<TMP_Text>().text = "Yes, do you have a quest for me?";

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

    IEnumerator EndConversation()
    {
        NPCBox.SetActive(false);
        isTalking = false;
        animator.SetBool("isTalking", false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        animator.SetBool("isWalking", true);

        yield return new WaitForSeconds(3.0f);

        hasTalked = false;
    }
}
