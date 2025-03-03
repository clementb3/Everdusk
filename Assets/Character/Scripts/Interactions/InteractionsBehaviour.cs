using UnityEngine;

public class InteractionsBehaviour : GenericBehaviour
{
    [SerializeField]
    private Inventory inventory;                  // Players' inventory
    private AnimationManager animationManager;    // Management of the different animations.
    [SerializeField]
    private MoveBehaviour moveBehaviour;          // Reference to the players' movement behaviour.
    private Item currentItem;                     // Reference to the item picked.
    
    [HideInInspector]
    public bool isBusy = false;
    void Start()
    {
        animationManager = new AnimationManager(behaviourManager.GetAnim);
    }

    // Pickup intercations.
    public void Pickup (Item pickupItem)
    {
        if(isBusy)
            return;
        ChangeMovement();
        currentItem = pickupItem;
        animationManager.Pickup();
    }

    public void AddItemToInventory()
    {
        if (inventory == null)
        {
            Debug.LogError("Inventory not assigned in InteractionsBehaviour!");
            return;
        }
        inventory.AddItem(currentItem.item);
        Destroy(currentItem.gameObject);
        currentItem = null;
    }

    // Intercactions with chests.
    public void OpenChest(Collider col)
    {
        if(isBusy)
            return;
        col.GetComponent<Animator>().SetTrigger("Activate");
        col.GetComponent<OtherInventory>().SendMessage("OpenInventory");
        Inventory.instance.SendMessage("OpenInventory");
        ChangeMovement();
    }

    public void CloseChest(Collider col)
    {
        col.GetComponent<Animator>().SetTrigger("Activate");
        col.GetComponent<OtherInventory>().SendMessage("CloseInventory");
        Inventory.instance.SendMessage("CloseInventory");
        ChangeMovement();
    }

    // General changes during interactions.
    public void ChangeMovement()
    {
        moveBehaviour.CanMove();
        isBusy = !isBusy;
        if (isBusy)
        {
            // Stop players' movement.
            moveBehaviour.StopMovement();
        }
    }
}
