using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private List<ItemData> content = new();
    public void AddItem(ItemData item)
    {
        content.Add(item);
    }
}
