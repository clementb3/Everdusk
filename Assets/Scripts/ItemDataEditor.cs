using UnityEditor;
using UnityEngine;

// Class to custom the appearance of the itemData inspector.
[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ItemData item = (ItemData)target;

        // Common properties.
        item.name = EditorGUILayout.TextField("Name", item.name);
        item.visual = (Sprite)EditorGUILayout.ObjectField("Visual", item.visual, typeof(Sprite), false);
        item.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", item.prefab, typeof(GameObject), false);
        item.itemType = (ItemType)EditorGUILayout.EnumPopup("Item Type", item.itemType);

        if (item.itemType == ItemType.Equipment)
        {
            // If the item is an equipment it displays more information.
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Equipment Settings", EditorStyles.boldLabel);

            item.equipmentType = (EquipmentType)EditorGUILayout.EnumPopup("Equipment Type", item.equipmentType);
            item.isEquipped = EditorGUILayout.Toggle("Is Equipped", item.isEquipped);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(item);
        }
    }
}
