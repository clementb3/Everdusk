using UnityEngine;

// Script used by the buttons in the inventories.
public class ItemChooser : MonoBehaviour
{
    [HideInInspector]
    public ItemData item;                        // Reference to the item chosen.

    public void ChooseItem()
    {
        // If the button clicked is in the players' inventory : 
        if (OtherInventory.currentInventory == null)
        {
            // If the button clicked corresponds to a new item.
            if (item != ItemActionSystem.instance.GetSelectedItem())
                ItemActionSystem.instance.OpenItemAction(item);
            // If it's the same item already choosen.
            else
                ItemActionSystem.instance.CloseItemAction();
        }
        // If it's in an other inventory :
        else
        {
            Transform buttonParent = transform.parent;
            // If we clicked in the players' inventory :
            if (buttonParent.CompareTag("PlayerInventory"))
            {
                // If the button clicked corresponds to a new item.
                if (item != ItemActionSystem.instance.GetSelectedItem())
                    ItemActionSystem.instance.OpenItemAction(item);
                // If it's the same item already choosen.
                else
                    ItemActionSystem.instance.CloseItemAction();
            }
            // If we clicked in the other inventory :
            else
            {
                // Check if we are with a merchant.
                bool isMerchant = OtherInventory.currentInventory.gameObject.CompareTag("Merchant");
                OtherItemActionSystem otherItemActionSystem = OtherInventory.currentInventory.GetOtherItemActionSystem();
                if (otherItemActionSystem != null)
                    if (item != otherItemActionSystem.GetSelectedItem())
                        otherItemActionSystem.OpenItemAction(item, isMerchant);
                    else
                        otherItemActionSystem.CloseItemAction();
                else
                    Debug.LogError("OtherItemActionSystem not found in the scene!");
            }
        }
    }
}
