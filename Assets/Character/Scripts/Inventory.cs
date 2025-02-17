using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private List<ItemData> content = new();

    [SerializeField]
    private GameObject displayedInventory;

    [SerializeField]
    private GameObject displayedContent;

    [SerializeField]
    private GameObject button;

    void Start()
    {
        AddItems();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
            displayedInventory.SetActive(!displayedInventory.activeSelf);
    }

    public void AddItems()
    {
        for(int i = 0; i<content.Count; i++)
            AddMenuItem(content[i]);
    }
    public void AddItem(ItemData item)
    {
        content.Add(item);
        AddMenuItem(item);
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
    }
}
