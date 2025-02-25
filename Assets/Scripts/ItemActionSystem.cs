using UnityEngine;

// Class referencing every actions of inventory action panel.
public class ItemActionSystem : MonoBehaviour
{
    [Header("SCRIPT REFERENCES")]
    [SerializeField]
    private Equipment equipment;                  // Reference to the equipment class.

    [Header("ACTION PANEL REFERENCES")]
    [SerializeField]
    private GameObject itemActions;               // Reference of the item action panel displayed.
    [SerializeField]
    private GameObject useItem;                   // Reference of the use button in the panel.
    [SerializeField]
    private GameObject equipItem;                 // Reference of the equip button in the panel.
    [SerializeField]
    private GameObject unequipItem;               // Reference of the equip button in the panel.
    [SerializeField]
    private GameObject dropItem;                  // Reference of the drop button in the panel.
    [SerializeField]
    private Transform dropPoint;                  // Reference to the point where items are dropped.
    private ItemData selectedItem;                // Reference to the selected item in the panel.
    public static ItemActionSystem instance;      // Self reference to be used in ItemChooser.

    void Start()
    {
        instance = this;
    }
    // Open the action panel of an item.
    public void OpenItemAction(ItemData item)
    {
        selectedItem = item;
        if (item == null)
            return;

        switch (item.itemType)
        {
            case ItemType.Ressource:
                useItem.SetActive(false);
                equipItem.SetActive(false);
                unequipItem.SetActive(false);
                break;
            case ItemType.Consumable:
                equipItem.SetActive(false);
                unequipItem.SetActive(false);
                useItem.SetActive(true);
                break;
            case ItemType.Equipment:
                if (item.isEquipped)
                {
                    unequipItem.SetActive(true);
                    equipItem.SetActive(false);
                }
                else
                {
                    equipItem.SetActive(true);
                    unequipItem.SetActive(false);
                }
                useItem.SetActive(false);
                break;
        }
        itemActions.SetActive(true);
    }

    // Close the action panel.
    public void CloseItemAction()
    {
        itemActions.SetActive(false);
        selectedItem = null;
    }

    // Functions to do actions on items.
    // Function called by the useItem button.
    public void UseItem()
    {
        CloseItemAction();
    }

    // Function called by the dropItem button.
    public void DropItem()
    {
        if (selectedItem.isEquipped)
        {
            EquipmentItem equipmentItem = equipment.GetEquipmentItem(selectedItem);
            if (equipmentItem != null)
                equipment.UnequipItem(equipmentItem);
        }
        GameObject droppedItem = Instantiate(selectedItem.prefab);
        droppedItem.transform.position = dropPoint.position;
        Inventory.instance.RemoveItem(selectedItem);
        Inventory.instance.RefreshContent();
        CloseItemAction();
    }

    // Getter.
    public ItemData GetSelectedItem()
    {
        return selectedItem;
    }

    public void SetSelectedItem(ItemData item)
    {
        selectedItem = item;
    }
}
