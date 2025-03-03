using System.Collections.Generic;
using UnityEngine;

// Class referencing every actions of players' inventory action panel.
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
    [HideInInspector]
    public bool isActive = false;

    void Start()
    {
        instance = this;
    }
    // Open the action panel of an item.
    public void OpenItemAction(ItemData item)
    {
        if (item == null)
            return;

        selectedItem = item;
        isActive = true;

        if (OtherItemActionSystem.currentItemAction != null)
            OtherItemActionSystem.currentItemAction.CloseItemAction();

        switch (item.itemType)
        {
            case ItemType.Resource:
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
        isActive = false;
    }

    // Functions to do actions on items.
    // Function called by the useItem button.
    public void UseItem()
    {
        // Function that will allow to use consumables like potions, food etc.
        CloseItemAction();
    }

    // Function called by the dropItem button.
    public void DropItem()
    {
        if (selectedItem.isEquipped)
        {
            ItemStacked itemStacked = Inventory.instance.GetItemStacked(selectedItem);
            if (itemStacked.count == 1)
            {
                EquipmentItem equipmentItem = equipment.GetEquipmentItem(selectedItem);
                if (equipmentItem != null)
                    equipment.UnequipItem(equipmentItem);
            }
        }
        GameObject droppedItem = Instantiate(selectedItem.prefab);
        droppedItem.transform.position = dropPoint.position;
        Inventory.instance.RemoveItem(selectedItem);
        CloseItemAction();
    }

    // Getter.
    public ItemData GetSelectedItem()
    {
        return selectedItem;
    }

    // Setter.
    public void SetSelectedItem(ItemData item)
    {
        selectedItem = item;
    }
}
