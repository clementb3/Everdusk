using UnityEngine;

// Class used to assign data for items in the scene.
[CreateAssetMenu(fileName = "Item", menuName = "Items/New Item")]
public class ItemData : ScriptableObject
{
    [Header("DATA")]
    public new string name;
    public Sprite visual;
    public GameObject prefab;
    public ItemType itemType;

    [Header("TYPE")]
    public EquipmentType equipmentType;
    public bool isEquipped = false;

    [Header("EQUIPMENT STATS")]
    public float armorPoints;
    public float weaponDamage;
}

public enum ItemType
{
    Resource,
    Consumable,
    Equipment
}

public enum EquipmentType
{
    Head,
    Chest,
    Hands,
    Legs,
    Feet, 
    Weapon
}