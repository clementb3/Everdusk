using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("INVENTORY REFERENCES")]
    [SerializeField]
    private List<ItemStacked> content = new();    // List of the items in the inventory.
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

    private bool canOpen = true;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (canOpen)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (isInventoryOpen)
                    CloseInventory();
                else
                {
                    OpenInventory();
                }
            }
        }
    }

    // Functions to display the inventory.
    private void OpenInventory()
    {
        displayedInventory.SetActive(true);
        isInventoryOpen = true;
        RefreshContent();
    }

    private void CloseInventory()
    {
        displayedInventory.SetActive(false);
        itemActionSystem.CloseItemAction();
        isInventoryOpen = false;
    }

    // Functions to fill/refresh the displayed inventory.
    public void AddMenuItem(ItemStacked item)
    {
        // Instantiate a new button with the referenced test button.
        GameObject newItem = Instantiate(button, transform.position, transform.rotation);
        // Set the content object as its parent. 
        newItem.transform.SetParent(displayedContent.transform, true);
        // Set the scale of the new button at 0 (on x, y and z).
        newItem.transform.localScale = Vector3.one;
        // Adapt its name.
        string name = $"  {item.itemData.name}";
        newItem.name = name;
        if (item.count > 1)
            name += $" ({item.count})";
        newItem.SetActive(true);
        newItem.GetComponentInChildren<Text>().text = item.itemData.isEquipped ? name + " [Equipped]" : name;
        newItem.GetComponent<ItemChooser>().item = item.itemData;
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

    // Function to manage the incentory content.
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
        Debug.Log(itemStacked);
        if (itemStacked.count > count)
            itemStacked.count -= count;
        else
            content.Remove(itemStacked);
        RefreshContent();
    }

    // Getter
    public ItemStacked GetItemStacked(ItemData itemData)
    {
        return content.Where(elem => elem.itemData == itemData).FirstOrDefault();
    }

    public List<ItemStacked> GetContent()
    {
        return content;
    }

    public Equipment GetEquipment()
    {
        return equipment;
    }

    // Setter
    public void CanOpen()
    {
        canOpen = !canOpen;
    }
}

[System.Serializable]
public class ItemStacked
{
    public ItemData itemData;
    public int count;
}