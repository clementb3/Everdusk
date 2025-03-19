using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OtherInventory : MonoBehaviour
{
    [Header("INVENTORY REFERENCES")]
    [SerializeField]
    private List<ItemStacked> content = new();             // List of the items in the inventory.
    public static OtherInventory currentInventory;         // Dynamic instance of the current inventory.

    [Header("DISPLAY INVENTORY REFERENCES")]
    [SerializeField]
    private GameObject displayedInventory;                 // Reference to the image of the inventory.
    [SerializeField]
    private GameObject displayedContent;                   // Reference to the set of buttons.
    [SerializeField]
    private GameObject button;                             // Reference to the duplicate button representing an item.

    [Header("SCRIPT REFERENCES")]
    [SerializeField]
    private OtherItemActionSystem otherItemActionSystem;   // Reference to the itemActionSystem class.

    void Awake()
    {
        GameObject displayedInventoryTmp = GameObject.Find("OtherInventoryList");
        GameObject displayedContentTmp = GameObject.Find("OtherContent");
        GameObject buttonTmp = GameObject.Find("OtherButton");

        displayedInventory = displayedInventoryTmp;
        displayedContent = displayedContentTmp;
        button = buttonTmp;
    }
    void Start()
    {
        displayedInventory.SetActive(false);
        button.SetActive(false);
    }

    // Functions to display the inventory.
    private void OpenInventory()
    {
        displayedInventory.SetActive(true);
        currentInventory = this;
        AddItems();
    }

    private void CloseInventory()
    {
        displayedInventory.SetActive(false);
        otherItemActionSystem.CloseItemAction();
        print(otherItemActionSystem);
        if (!OtherItemActionSystem.currentItemAction && !ItemActionSystem.instance.isInspecting)
            currentInventory = null;
        RemoveItems();
    }

    // Functions to fill/refresh the inventory displayed.
    public void AddItems()
    {
        for (int i = 0; i < content.Count; i++)
            AddMenuItem(content[i]);
    }

    // Remove the displayed items from the displayed inventory.
    public void RemoveItems()
    {
        for (int i = displayedContent.transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = displayedContent.transform.GetChild(i).gameObject;
            if (child != button)
            {
                Destroy(child);
            }
        }
    }

    public void AddMenuItem(ItemStacked item)
    {
        GameObject newItem = Instantiate(button, displayedContent.transform);
        newItem.transform.localRotation = Quaternion.identity;
        newItem.transform.SetParent(displayedContent.transform, true);
        string name = $"  {item.itemData.name}";
        newItem.name = name;
        if (item.count > 1)
            name += $" ({item.count})";
        newItem.SetActive(true);
        newItem.GetComponentInChildren<Text>().text = name;
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

    // Function called to manage the content in the inventory.
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

    // Getter
    public ItemStacked GetItemStacked(ItemData itemData)
    {
        return content.Where(elem => elem.itemData == itemData).FirstOrDefault();
    }

    public List<ItemStacked> GetContent()
    {
        return content;
    }

    public OtherItemActionSystem GetOtherItemActionSystem()
    {
        return otherItemActionSystem;
    }
}