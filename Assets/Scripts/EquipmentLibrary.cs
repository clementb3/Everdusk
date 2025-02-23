using System.Collections.Generic;
using UnityEngine;

public class EquipmentLibrary : MonoBehaviour
{
    public List<EquipmentItem> content = new();
}

[System.Serializable]
public class EquipmentItem
{
    public ItemData itemData;
    public GameObject prefab;
    public GameObject[] disableItem;
}