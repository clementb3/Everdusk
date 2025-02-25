using System.Linq;
using UnityEngine;

// Class referencing different equipment variables.
public class Equipment : MonoBehaviour
{
    [Header("SCRIPT REFERENCES")]
    [SerializeField]
    private ItemActionSystem itemActionSystem;    // Reference to the itemActionSystem class.
    [SerializeField]
    private EquipmentLibrary equipmentLibrary;    // Reference to the list of equipment used to equip items from the inventory.
    
    // References to equipped items;
    private ItemData equippedHead;                // Reference to the actual head equipment.
    private ItemData equippedLegs;                // Reference to the actual legs equipment.
    private ItemData equippedHands;               // Reference to the actual hands equipment.
    private ItemData equippedChest;               // Reference to the actual chest equipment.
    private ItemData equippedFeet;                // Reference to the actual feet equipment.
    private ItemData equippedWeapon;              // Reference to the actual weapon equipped.

    // Function called by the equip/unequip button to equip/unequip an item.
    public void EquipItem()
    {
        EquipmentItem equipmentItem = GetEquipmentItem(itemActionSystem.GetSelectedItem());
        if (equipmentItem != null)
        {
            if (itemActionSystem.GetSelectedItem().isEquipped)
                UnequipItem(equipmentItem);
            else
            {
                switch (itemActionSystem.GetSelectedItem().equipmentType)
                {
                    case EquipmentType.Head:
                        DisablePreviousEquipment(equippedHead);
                        equippedHead = itemActionSystem.GetSelectedItem();
                        break;
                    case EquipmentType.Chest:
                        DisablePreviousEquipment(equippedChest);
                        equippedChest = itemActionSystem.GetSelectedItem();
                        break;
                    case EquipmentType.Hands:
                        DisablePreviousEquipment(equippedHands);
                        equippedHands = itemActionSystem.GetSelectedItem();
                        break;
                    case EquipmentType.Legs:
                        DisablePreviousEquipment(equippedLegs);
                        equippedLegs = itemActionSystem.GetSelectedItem();
                        break;
                    case EquipmentType.Feet:
                        DisablePreviousEquipment(equippedFeet);
                        equippedFeet = itemActionSystem.GetSelectedItem();
                        break;
                    case EquipmentType.Weapon:
                        DisablePreviousEquipment(equippedWeapon);
                        equippedWeapon = itemActionSystem.GetSelectedItem();
                        break;
                }
                for (int i = 0; i < equipmentItem.disableItem.Length; i++)
                    equipmentItem.disableItem[i].SetActive(false);
                equipmentItem.prefab.SetActive(true);
                itemActionSystem.GetSelectedItem().isEquipped = true;
                Inventory.instance.RefreshContent();
            }
        }
        else
            Debug.LogError(itemActionSystem.GetSelectedItem() + " is not in the list of equipment");
        itemActionSystem.CloseItemAction();
    }

    public void UnequipItem(EquipmentItem equipmentItem)
    {
        for (int i = 0; i < equipmentItem.disableItem.Length; i++)
            equipmentItem.disableItem[i].SetActive(true);
        equipmentItem.prefab.SetActive(false);
        itemActionSystem.GetSelectedItem().isEquipped = false;
        Inventory.instance.RefreshContent();
    }

    // Unequip an equipment before wearing another one.
    private void DisablePreviousEquipment(ItemData item)
    {
        if (item == null)
            return;
        EquipmentItem equipmentItem = GetEquipmentItem(item);
        for (int i = 0; i < equipmentItem.disableItem.Length; i++)
            equipmentItem.disableItem[i].SetActive(true);
        equipmentItem.prefab.SetActive(false);
        item.isEquipped = false;
        Inventory.instance.RefreshContent();
    }

    // Select the item to equip/unequip, its prefab and the corresponding basics items' prefabs.
    public EquipmentItem GetEquipmentItem(ItemData item)
    {
        return equipmentLibrary.content.FirstOrDefault(equip => equip.itemData == item);
    }
}
