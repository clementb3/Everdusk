using UnityEngine;
using UnityEngine.UI;

// Class referencing every actions of players' inventory action panel.
public class ItemActionSystem : MonoBehaviour
{
    [Header("SCRIPT REFERENCES")]
    [SerializeField]
    private Equipment equipment;                  // Reference to the equipment class.

    [Header("ACTION PANEL REFERENCES")]
    [SerializeField]
    private GameObject itemActions;               // Reference of the item action panel displayed.
    [SerializeField]
    private GameObject useItem;                   // Reference of the use button in the panel.
    [SerializeField]
    private GameObject equipItem;                 // Reference of the equip button in the panel.
    [SerializeField]
    private GameObject unequipItem;               // Reference of the equip button in the panel.
    [SerializeField]
    private GameObject dropItem;                  // Reference of the drop button in the panel.
    [SerializeField]
    private GameObject inspectItem;               // Reference of the inspect button in the panel.
    [SerializeField]
    private Transform dropPoint;                  // Reference to the point where items are dropped.

    [Header("ITEM DETAILS REFERENCES")]
    [SerializeField]
    private GameObject itemDetailPanel;           // Reference to the panel displaying item details.
    [SerializeField]
    private Text itemName;                        // Reference to the Text in the panel for the item name.
    [SerializeField]
    private Text itemTypeStat;                    // Reference to the Text in the panel for the item type stat (armor or damage).
    [SerializeField]
    private Text itemStat;                        // Reference to the Text in the panel for the item stat.

    [Header("ITEM INSPECTION REFERENCE")]
    [SerializeField]
    private Transform inspectionPoint;            // Point where the object will be displayed.
    [SerializeField]
    private GameObject mainCamera;                // Reference to the camera.
    [SerializeField]
    private GameObject inspectionCamera;          // Reference to the camera.
    [SerializeField]
    private MoveBehaviour moveBehaviour;          // Reference to the moveBehaviour script to enable/disable players' movement.
    private float rotationSpeed = 100f;           // Speed of the inspected item rotation.
    private Vector3 previousMousePosition;        // Reference to the last position of the mouse to rotate the item.

    // Other references. 
    private ItemData selectedItem;                // Reference to the selected item in the panel.
    public static ItemActionSystem instance;      // Self reference to be used in ItemChooser.
    [HideInInspector]
    public bool isActive = false;                 // Boolean to check if an action panel of the players' inventory is open.
    [HideInInspector]
    public bool isInspecting = false;                    // Boolean to check if the player is inspecting an object.

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (isInspecting)
        {
            RotateItem();
        }
    }

    // Open the action panel of an item.
    public void OpenItemAction(ItemData item)
    {
        if (item == null)
            return;

        selectedItem = item;
        isActive = true;

        if (OtherItemActionSystem.currentItemAction != null)
            OtherItemActionSystem.currentItemAction.CloseItemAction();

        switch (item.itemType)
        {
            case ItemType.Resource:
                useItem.SetActive(false);
                equipItem.SetActive(false);
                unequipItem.SetActive(false);
                break;
            case ItemType.Consumable:
                equipItem.SetActive(false);
                unequipItem.SetActive(false);
                useItem.SetActive(true);
                break;
            case ItemType.Equipment:
                if (item.isEquipped)
                {
                    unequipItem.SetActive(true);
                    equipItem.SetActive(false);
                }
                else
                {
                    equipItem.SetActive(true);
                    unequipItem.SetActive(false);
                }
                useItem.SetActive(false);
                break;
        }
        itemActions.SetActive(true);
        OpenItemDetail();
    }

    // Close the action panel.
    public void CloseItemAction()
    {
        itemActions.SetActive(false);
        if (!isInspecting)
            selectedItem = null;
        isActive = false;
        CloseItemDetail();
    }

    // Open the item details panel.
    void OpenItemDetail()
    {
        itemTypeStat.text = "";
        itemStat.text = "";
        if (selectedItem.itemType == ItemType.Equipment)
        {
            // If the item is an equipment...
            if (selectedItem.equipmentType == EquipmentType.Weapon)
            {
                // If it is a weapon add its damages.
                itemStat.text = $"{selectedItem.weaponDamage}";
                itemTypeStat.text = "Damage";
            }
            else
            {
                // If it is an armor add its protection points.
                itemStat.text = $"{selectedItem.armorPoints}";
                itemTypeStat.text = "Armor";
            }
        }
        // Put the name of the item on the panel.
        itemName.text = selectedItem.name;
        itemDetailPanel.SetActive(true);
    }

    void CloseItemDetail()
    {
        itemDetailPanel.SetActive(false);
    }

    // Functions to do actions on items.
    // Function called by the useItem button.
    public void UseItem()
    {
        // Function that will allow to use consumables like potions, food etc.
        CloseItemAction();
    }

    // Function called by the inspectButton.
    public void InspectItem()
    {
        Inventory.instance.CanOpen();
        // Place the inspection point correctly.
        inspectionPoint.SetPositionAndRotation(inspectionCamera.transform.position + inspectionCamera.transform.forward * 1.5f, inspectionCamera.transform.rotation);
        // Correctly rotate the point.
        inspectionPoint.Rotate(Vector3.right, 180f);
        inspectionPoint.Rotate(Vector3.forward, 180f);
        // Enable the camera used for item inspection.
        inspectionCamera.SetActive(true);
        // Disable the main camera.
        mainCamera.SetActive(false);
        // Disable players' movement.
        moveBehaviour.CanMove();
        isInspecting = true;
        // Calcultate the item size.
        GameObject item = Instantiate(selectedItem.prefab, inspectionPoint.position, inspectionPoint.rotation, inspectionPoint);
        Renderer[] renderers = item.GetComponentsInChildren<Renderer>();

        if (renderers.Length > 0)
        {
            Bounds bounds = renderers[0].bounds;
            foreach (Renderer rend in renderers)
            {
                bounds.Encapsulate(rend.bounds);  // Extend bounds to encapsulate the whole item.
            }

            // Shift the item to center its pivot.
            Vector3 centerOffset = bounds.center - item.transform.position;
            item.transform.position -= centerOffset;
        }

        Rigidbody rb = item.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        Inventory.instance.SendMessage("CloseInventory");
    }

    void RotateItem()
    {
        if (Input.GetKey(KeyCode.Escape))
            StopInspectItem();
        if (Input.GetMouseButtonDown(0))
            previousMousePosition = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            // Calculate the difference between the last and the new mouse position.
            Vector3 deltaMouse = Input.mousePosition - previousMousePosition;
            // Calculate the rotation of the item.
            float rotationX = deltaMouse.y * rotationSpeed * Time.deltaTime;
            float rotationY = -deltaMouse.x * rotationSpeed * Time.deltaTime;
            // Apply this rotation on the item.
            Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
            inspectionPoint.rotation = rotation * inspectionPoint.rotation;
            // Update the mouse position.
            previousMousePosition = Input.mousePosition;
        }
    }

    public void StopInspectItem()
    {
        if (inspectionPoint.childCount > 0)
            foreach (Transform child in inspectionPoint)
                Destroy(child.gameObject);
        isInspecting = false;
        mainCamera.SetActive(true);
        inspectionCamera.SetActive(false);
        OpenItemAction(selectedItem);
        // Enable players' movement.
        moveBehaviour.CanMove();
        Inventory.instance.CanOpen();
        Inventory.instance.SendMessage("OpenInventory");
    }

    // Function called by the dropItem button.
    public void DropItem()
    {
        if (selectedItem.isEquipped)
        {
            ItemStacked itemStacked = Inventory.instance.GetItemStacked(selectedItem);
            if (itemStacked.count == 1)
            {
                EquipmentItem equipmentItem = equipment.GetEquipmentItem(selectedItem);
                if (equipmentItem != null)
                    equipment.UnequipItem(equipmentItem);
            }
        }
        GameObject droppedItem = Instantiate(selectedItem.prefab);
        droppedItem.transform.position = dropPoint.position;
        Inventory.instance.RemoveItem(selectedItem);
        CloseItemAction();
    }

    // Getter.
    public ItemData GetSelectedItem()
    {
        return selectedItem;
    }

    // Setter.
    public void SetSelectedItem(ItemData item)
    {
        selectedItem = item;
    }
}
