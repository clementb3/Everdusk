using UnityEngine;

// Component used to assign data for items in the scene.
[CreateAssetMenu(fileName = "Item", menuName = "Items/New Item")]
public class ItemData : ScriptableObject
{
    public new string name;
    public Sprite visual;
    public GameObject prefab;
    public ItemType itemType;
    public EquipmentType equipmentType;
}

public enum ItemType
{
    Ressource,
    Consumable,
    Equipment
}

public enum EquipmentType
{
    Head,
    Chest,
    Hands,
    Legs,
    Feet
}
