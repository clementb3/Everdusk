using UnityEngine;
using UnityEngine.UI;

public class OtherItemActionSystem : MonoBehaviour
{
    private GameObject itemActions;                             // Reference of the item action panel displayed.
    private Button takeItem;                                    // Reference of the take button in the panel.
    private Button equipItem;                                   // Reference of the equip button in the panel (chest only).
    private Button buyItem;                                     // Reference of the buy button in the panel (npc only).
    private ItemData selectedItem;                              // Reference to the selected item in the panel.
    public static OtherItemActionSystem currentItemAction;      // Dynamic instance of the current action panel.

    void Awake()
    {
        GameObject itemActionsTmp = GameObject.Find("OtherItemActions");
        Button takeItemTmp = GameObject.Find("TakeItem").GetComponent<Button>();
        Button equipItemTmp = GameObject.Find("OtherEquipItem").GetComponent<Button>();
        Button buyItemTmp = GameObject.Find("BuyItem").GetComponent<Button>();

        itemActions = itemActionsTmp;
        takeItem = takeItemTmp;
        equipItem = equipItemTmp;
        buyItem = buyItemTmp;
    }

    void Start()
    {
        itemActions.SetActive(false);
    }

    public void OpenItemAction(ItemData item, bool isMerchant)
    {
        selectedItem = item;

        if (currentItemAction != null)
            currentItemAction = null;

        currentItemAction = this;

        if (ItemActionSystem.instance.isActive)
            ItemActionSystem.instance.CloseItemAction();

        if (item == null)
            return;

        if (takeItem != null)
            takeItem.onClick.RemoveAllListeners();
        if (equipItem != null)
            equipItem.onClick.RemoveAllListeners();
        if (buyItem != null)
            buyItem.onClick.RemoveAllListeners();

        // Activate the appropriate buttons based on inventory type
        takeItem.gameObject.SetActive(!isMerchant);
        equipItem.gameObject.SetActive(!isMerchant && item.itemType == ItemType.Equipment);
        buyItem.gameObject.SetActive(isMerchant);

        // Add the correct listeners.
        if (takeItem != null)
            takeItem.onClick.AddListener(TakeItem);
        if (equipItem != null)
            equipItem.onClick.AddListener(EquipItem);
        if (buyItem != null)
            buyItem.onClick.AddListener(BuyItem);

        itemActions.SetActive(true);
    }

    // Close the action panel.
    public void CloseItemAction()
    {
        itemActions.SetActive(false);
        selectedItem = null;
        currentItemAction = null;
    }

    // Function called by the takeItem button.
    public void TakeItem()
    {
        if (selectedItem == null || OtherInventory.currentInventory == null)
            return;
        Inventory.instance.AddItem(selectedItem);
        OtherInventory.currentInventory.RemoveItem(selectedItem);
        CloseItemAction();
    }

    public void EquipItem()
    {
        if (selectedItem == null || OtherInventory.currentInventory == null)
            return;

        ItemActionSystem.instance.SetSelectedItem(selectedItem);
        Inventory.instance.AddItem(selectedItem);
        Inventory.instance.GetEquipment().EquipItem();
        OtherInventory.currentInventory.RemoveItem(selectedItem);

        CloseItemAction();
    }

    // Function called by the buyItem button.
    public void BuyItem()
    {
        Debug.Log("Buying system not implemented yet.");
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
