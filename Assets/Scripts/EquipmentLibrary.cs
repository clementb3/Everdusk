using System.Collections.Generic;
using UnityEngine;

// Class used to link an item with its prefab displayed in the scene and with the corresponding basic(s) prefab(s)
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