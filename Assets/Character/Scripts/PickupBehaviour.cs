using UnityEngine;

public class PickupBehaviour : GenericBehaviour
{
    [SerializeField]
    public Inventory inventory;

    private AnimationManager animationManager;    // Management of the different animations.
    [SerializeField]
    private MoveBehaviour moveBehaviour;          // Reference to the players' movement behaviour.
    private Item currentItem;                     // Reference to the item picked.
    void Start()
    {
        animationManager = new AnimationManager(behaviourManager.GetAnim);
    }

    public void Pickup (Item pickupItem)
    {
        currentItem = pickupItem;
        animationManager.Pickup();
        ChangeMovement();
    }

    public void AddItemToInventory()
    {
        if (inventory == null)
        {
            Debug.LogError("Inventory not assigned in PickupBehaviour!");
            return;
        }
        inventory.AddItem(currentItem.item);
        Destroy(currentItem.gameObject);
        currentItem = null;
    }

    public void ChangeMovement()
    {
        moveBehaviour.CanMove();
    }
}
