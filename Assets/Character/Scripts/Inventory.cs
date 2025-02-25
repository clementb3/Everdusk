using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("INVENTORY REFERENCES")]
    [SerializeField]
    private List<ItemStacked> content = new();       // List of the items in the inventory.
    public static Inventory instance;             // Reference of the inventory.

    [Header("DISPLAY INVENTORY REFERENCES")]
    [SerializeField]
    private GameObject displayedInventory;        // Reference to the image of the inventory.
    [SerializeField]
    private GameObject displayedContent;          // Reference to the set of buttons.
    [SerializeField]
    private GameObject button;                    // Reference to the duplicate button representing an item.
    private bool isInventoryOpen = false;         // Bool to manage the inventory.

    [Header("SCRIPT REFERENCES")]
    [SerializeField]
    private Equipment equipment;                  // Reference to the equipment class.
    [SerializeField]
    private ItemActionSystem itemActionSystem;    // Reference to the itemActionSystem class.

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
        {
            if (isInventoryOpen)
                CloseInventory();
            else
                OpenInventory();
        }
    }

    // Functions to display the inventory.
    private void OpenInventory()
    {
        displayedInventory.SetActive(true);
        isInventoryOpen = true;
    }

    private void CloseInventory()
    {
        displayedInventory.SetActive(false);
        itemActionSystem.CloseItemAction();
        isInventoryOpen = false;
    }

    // Functions to fill/refresh the inventory.
    // Only called once in Start() to fill the inventory if necessary.
    public void AddItems()
    {
        for (int i = 0; i < content.Count; i++)
            AddMenuItem(content[i]);
    }

    // Function called in the animation of pickup to add an item in the inventory.
    public void AddItem(ItemData item)
    {
        ItemStacked itemStacked = GetItemStacked(item);
        if (itemStacked != null)
            itemStacked.count++;
        else
            content.Add(new ItemStacked { itemData = item, count = 1 });
        RefreshContent();
    }

    public void RemoveItem(ItemData item, int count = 1)
    {
        ItemStacked itemStacked = GetItemStacked(item);
        if (itemStacked.count > count)
            itemStacked.count -= count;
        else
            content.Remove(itemStacked);
        RefreshContent();
    }

    // Function to add the item to the displayed inventory.
    public void AddMenuItem(ItemStacked item)
    {
        GameObject newItem = Instantiate(button, transform.position, transform.rotation);
        newItem.transform.SetParent(displayedContent.transform, true);
        string name = $"  {item.itemData.name}";
        newItem.name = name;
        if (item.count > 1)
            name += $" ({item.count})";
        newItem.SetActive(true);
        newItem.GetComponentInChildren<Text>().text = item.itemData.isEquipped ? name + " [Equipped]" : name;
        newItem.GetComponent<ItemChooser>().item = item.itemData;
    }

    // Function called to refresh the displayed inventory.
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

    // Getter
    public ItemStacked GetItemStacked(ItemData itemData)
    {
        return content.Where(elem => elem.itemData == itemData).FirstOrDefault();
    }
}

[System.Serializable]
public class ItemStacked
{
    public ItemData itemData;
    public int count;
}