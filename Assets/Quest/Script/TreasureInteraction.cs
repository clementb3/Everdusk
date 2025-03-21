using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro; // Assure-toi d’avoir ce "using"


public class TreasureInteraction : MonoBehaviour
{
    public Animator chestAnimator; // Référence à l'Animator du coffre
    public GameObject diamond; // L'objet du diamant
    public Transform museumDropPoint; // Où le diamant doit être déposé
    public GameObject museumTrigger; // La zone de détection dans le musée
    public Image diamondUI; // Référence à l'image UI du diamant


    private bool isChestOpened = false;
    private bool isNearTreasure = false;
    private bool hasDiamond = false;
    private bool isNearMuseum = false;

    public TMP_Text questStatusLabel; // Assigne-le dans l’Inspector

    void Start()
    { 
        // S'assurer que le diamant est désactivé au départ
        diamond.SetActive(false);
    }

    void Update()
    {
        /*
        #if UNITY_EDITOR
        // Debug.Log($"isNearTreasure: {isNearTreasure}, hasDiamond: {hasDiamond}, isNearMuseum: {isNearMuseum}");
        #endif
        */

        // Si le joueur est proche du coffre et appuie sur "E", il récupère le diamant
        if (isNearTreasure && Input.GetKeyDown(KeyCode.E) && !hasDiamond)
        {
            PickupDiamond();
        }

        // Si le joueur est dans le musée et appuie sur "E", il dépose le diamant
        if (isNearMuseum && Input.GetKeyDown(KeyCode.E) && hasDiamond)
        {
            DropDiamond();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Vérifier si le joueur est entré dans la zone du coffre
        if (other.CompareTag("Player"))
        {
            // Ouvrir le coffre si ce n'est pas déjà fait
            if (!isChestOpened)
            {
                OpenChest();
            }
            isNearTreasure = true;
        }

        // Vérifier si le joueur est entré dans la zone du musée
        if (other.CompareTag("Museum"))
        {
            isNearMuseum = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearTreasure = false;

            if (!hasDiamond) 
            {
                CloseChest();
            }
        }

        if (other.gameObject == museumTrigger)
        {
            isNearMuseum = false;
        }
    }

    public void SetNearMuseum(bool state)
    {
        isNearMuseum = state;
    }

    void OpenChest()
    {
        chestAnimator.SetTrigger("Open"); // Joue l’animation d’ouverture
        isChestOpened = true;

        // Active le diamant pour qu’il soit visible et récupérable
        diamond.SetActive(true);
    }

    void CloseChest()
    {
        chestAnimator.SetTrigger("Close"); // Ajoute ce trigger dans l'Animator
        isChestOpened = false;
    }

    void PickupDiamond()
    {
        hasDiamond = true;
        diamond.SetActive(false); // Désactiver l'objet physique
        diamondUI.gameObject.SetActive(true); // Activer l'image UI
    }

    /*
    void DropDiamond()
    {
        hasDiamond = false;

        // Déplacer le diamant à la position du musée et le rendre visible
        diamond.transform.position = museumDropPoint.position;
        diamond.SetActive(true);

        diamondUI.gameObject.SetActive(false); // Désactiver l'image UI
    }*/

    void DropDiamond()
    {
        hasDiamond = false;

        diamond.transform.position = museumDropPoint.position;
        diamond.SetActive(true);
        diamondUI.gameObject.SetActive(false);

        questStatusLabel.text = "Quest completed!";
        questStatusLabel.gameObject.SetActive(true);

        StartCoroutine(HideQuestStatusLabelAfterDelay(3f));
    }

    IEnumerator HideQuestStatusLabelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        questStatusLabel.gameObject.SetActive(false);
    }

}