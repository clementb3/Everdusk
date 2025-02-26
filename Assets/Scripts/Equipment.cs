using System.Collections.Generic;
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
    [SerializeField]
    private PlayerStat playerStat;                // Reference to the player stat script.

    // References to equipped items;
    private ItemData equippedHead;                // Reference to the actual head equipment.
    private ItemData equippedLegs;                // Reference to the actual legs equipment.
    private ItemData equippedHands;               // Reference to the actual hands equipment.
    private ItemData equippedChest;               // Reference to the actual chest equipment.
    private ItemData equippedFeet;                // Reference to the actual feet equipment.
    private ItemData equippedWeapon;              // Reference to the actual weapon equipped.

    void Start()
    {
        EquipAll();
    }

    // Function called by the equip/unequip button to equip/unequip an item.
    public void EquipItem()
    {
        // Get the corresponding items to the item to equip.
        EquipmentItem equipmentItem = GetEquipmentItem(itemActionSystem.GetSelectedItem());
        if (equipmentItem != null)
        {
            // Item already equiped => unequip it.
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
                // Add the armor points of the equipment.
                playerStat.currentArmor += itemActionSystem.GetSelectedItem().armorPoints;
                // Unactive the corresponding items to the item to equip.
                for (int i = 0; i < equipmentItem.disableItem.Length; i++)
                    equipmentItem.disableItem[i].SetActive(false);
                // Active the item to equip.
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
        // Active the corresponding equipment to the item to unequip.
        for (int i = 0; i < equipmentItem.disableItem.Length; i++)
            equipmentItem.disableItem[i].SetActive(true);
        // Unactive the item to unequip.
        equipmentItem.prefab.SetActive(false);
        itemActionSystem.GetSelectedItem().isEquipped = false;
        // Remove the armor points of the equipment.
        playerStat.currentArmor -= equipmentItem.itemData.armorPoints;
        Inventory.instance.RefreshContent();
    }

    // Unequip an item before wearing another one.
    private void DisablePreviousEquipment(ItemData item)
    {
        // No previous equipment => return.
        if (item == null)
            return;
        // Else get the previous equipment.
        EquipmentItem equipmentItem = GetEquipmentItem(item);
        // Active the corresponding equipment to the item to disable.
        for (int i = 0; i < equipmentItem.disableItem.Length; i++)
            equipmentItem.disableItem[i].SetActive(true);
        // Unactive the previous equipment.
        equipmentItem.prefab.SetActive(false);
        item.isEquipped = false;
        // Remove the armor points of the previous equipment.
        playerStat.currentArmor -= item.armorPoints;
        Inventory.instance.RefreshContent();
    }

    // Select the item to equip/unequip, its prefab and the corresponding basics items' prefabs.
    public EquipmentItem GetEquipmentItem(ItemData item)
    {
        return equipmentLibrary.content.FirstOrDefault(equip => equip.itemData == item);
    }

    // Check if there is already a weapon equipped.
    public bool IsWeaponEquipped()
    {
        return equippedWeapon != null;
    }

    // Function called in start to equip potential equipped items in the inventory. 
    void EquipAll()
    {
        List<ItemStacked> content = Inventory.instance.GetContent();
        for (int i = 0; i < content.Count; i++)
        {
            if (content[i].itemData.isEquipped)
            {
                EquipmentItem equipmentItem = GetEquipmentItem(content[i].itemData);
                switch (content[i].itemData.equipmentType)
                {
                    case EquipmentType.Head:
                        equippedHead = content[i].itemData;
                        break;
                    case EquipmentType.Chest:
                        equippedChest = content[i].itemData;
                        break;
                    case EquipmentType.Hands:
                        equippedHands = content[i].itemData;
                        break;
                    case EquipmentType.Legs:
                        equippedLegs = content[i].itemData;
                        break;
                    case EquipmentType.Feet:
                        equippedFeet = content[i].itemData;
                        break;
                    case EquipmentType.Weapon:
                        equippedWeapon = content[i].itemData;
                        break;
                }
                // Add the armor points of the equipment.
                playerStat.currentArmor += content[i].itemData.armorPoints;
                // Unactive the corresponding items to the item to equip.
                for (int j = 0; j < equipmentItem.disableItem.Length; j++)
                    equipmentItem.disableItem[j].SetActive(false);
                // Active the item to equip.
                equipmentItem.prefab.SetActive(true);
                content[i].itemData.isEquipped = true;
                Inventory.instance.RefreshContent();
            }
        }
    }

    // Getter
    public ItemData GetEquippedWeapon()
    {
        return equippedWeapon;
    }
}
