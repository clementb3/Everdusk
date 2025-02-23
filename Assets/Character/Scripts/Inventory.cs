using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private List<ItemData> content = new();

    [SerializeField]
    private GameObject displayedInventory;        // Reference to the image of the inventory.
    [SerializeField]
    private GameObject displayedContent;          // Reference to the set of buttons.
    [SerializeField]
    private GameObject button;                    // Reference to the duplicate button representing an item.

    // References to the inventory and item action panel to manage possible actions on items.
    public static Inventory instance;             // Reference of the inventory.
    [SerializeField]
    private GameObject itemActions;               // Reference of the item action panel displayed.
    [SerializeField]
    private GameObject useItem;                   // Reference of the use button in the panel.
    [SerializeField]
    private GameObject equipItem;                 // Reference of the equip button in the panel.
    [SerializeField]
    private GameObject dropItem;                  // Reference of the drop button in the panel.
    private ItemData selectedItem;                // Reference to the selected item in the panel.
    [SerializeField]
    private Transform dropPoint;                  // Reference to the point where items are dropped.
    [SerializeField]
    private EquipmentLibrary equipmentLibrary;    // Reference to the list of equipment used to equip items from the inventory.

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        AddItems();
        displayedInventory.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            displayedInventory.SetActive(!displayedInventory.activeSelf);
    }

    public void AddItems()
    {
        for (int i = 0; i < content.Count; i++)
            AddMenuItem(content[i]);
    }
    public void AddItem(ItemData item)
    {
        content.Add(item);
        RefreshContent();
    }

    public void AddMenuItem(ItemData item)
    {
        GameObject newItem;
        string name = $"{item.name}";
        newItem = Instantiate(button, transform.position, transform.rotation);
        newItem.name = name;
        newItem.transform.SetParent(displayedContent.transform, true);
        newItem.SetActive(true);
        newItem.GetComponentInChildren<Text>().text = name;
        newItem.GetComponent<ItemChooser>().item = item;
    }

    public void RefreshContent()
    {
        for (int i = displayedContent.transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = displayedContent.transform.GetChild(i).gameObject;
            if (child != button)
            {
                Destroy(child);
            }
        }
        for (int i = 0; i < content.Count; i++)
            AddMenuItem(content[i]);
    }

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
                break;
            case ItemType.Consumable:
                equipItem.SetActive(false);
                useItem.SetActive(true);
                break;
            case ItemType.Equipment:
                useItem.SetActive(false);
                equipItem.SetActive(true);
                break;
        }
        itemActions.SetActive(true);
    }

    public void CloseItemAction()
    {
        itemActions.SetActive(false);
        selectedItem = null;
    }

    public ItemData GetSelecteditem()
    {
        return selectedItem;
    }

    public void UseItem()
    {
        CloseItemAction();
    }

    public void EquipItem()
    {
        EquipmentItem equipmentItem = equipmentLibrary.content.Where(elem => elem.itemData == selectedItem).First();
        if (equipmentItem != null)
        {
            for (int i = 0; i < equipmentItem.disableItem.Length; i++)
                equipmentItem.disableItem[i].SetActive(false);
            equipmentItem.prefab.SetActive(true);
        }
        else
            Debug.LogError(selectedItem + " is not in the list of equipment");
        CloseItemAction();
    }

    public void DropItem()
    {
        GameObject droppedItem = Instantiate(selectedItem.prefab);
        droppedItem.transform.position = dropPoint.position;
        content.Remove(selectedItem);
        RefreshContent();
        CloseItemAction();
    }
}
