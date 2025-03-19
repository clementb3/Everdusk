using UnityEngine;
using UnityEngine.UI;

public class OtherItemActionSystem : MonoBehaviour
{
    // Item action panel.
    [SerializeField]
    private GameObject itemActions;                             // Reference of the item action panel displayed.

    // Buttons on item action panel.
    [SerializeField]
    private Button takeItem;                                    // Reference of the take button in the panel.
    [SerializeField]
    private Button equipItem;                                   // Reference of the equip button in the panel (chest only).
    [SerializeField]
    private Button buyItem;                                     // Reference of the buy button in the panel (npc only).
    [SerializeField]
    private Button inspectItem;                                 // Reference of the inspect button in the panel.

    private ItemData selectedItem;                              // Reference to the selected item in the panel.

    // Item detail Panel
    [SerializeField]
    private GameObject itemDetailPanel;                         // Reference to the panel displaying item details.
    [SerializeField]
    private Text itemName;                                      // Reference to the Text in the panel for the item name.
    [SerializeField]
    private Text itemTypeStat;                                  // Reference to the Text in the panel for the item type stat (armor or damage).
    [SerializeField]
    private Text itemStat;                                      // Reference to the Text in the panel for the item stat.

    // References for the inspection.
    [SerializeField]
    private Transform inspectionPoint;                          // Point where the object will be displayed.
    [SerializeField]
    private GameObject mainCamera;                              // Reference to the camera.
    [SerializeField]
    private GameObject inspectionCamera;                        // Reference to the camera.
    [SerializeField]
    private MoveBehaviour moveBehaviour;                        // Reference to the moveBehaviour script to enable/disable players' movement.
    private float rotationSpeed = 100f;                         // Speed of the inspected item rotation.
    private Vector3 previousMousePosition;                      // Reference to the last position of the mouse to rotate the item.
    [HideInInspector]
    public bool isInspecting = false;                           // Boolean to check if the player is inspecting an object.

    // Other references.
    public static OtherItemActionSystem currentItemAction;      // Dynamic instance of the current action panel.
    bool isMerchantGlobal = false;
    void Awake()
    {
        // Setting the item action panel.
        GameObject itemActionsTmp = GameObject.Find("OtherItemActions");
        Button takeItemTmp = GameObject.Find("TakeItem").GetComponent<Button>();
        Button equipItemTmp = GameObject.Find("OtherEquipItem").GetComponent<Button>();
        Button buyItemTmp = GameObject.Find("BuyItem").GetComponent<Button>();
        Button inspectItemTmp = GameObject.Find("OtherInspectItem").GetComponent<Button>();
        itemActions = itemActionsTmp;
        takeItem = takeItemTmp;
        equipItem = equipItemTmp;
        buyItem = buyItemTmp;
        inspectItem = inspectItemTmp;

        // Setting the item details panel.
        GameObject itemDetailTmp = GameObject.Find("OtherInventoryItemDetails");
        Text itemNameTmp = GameObject.Find("OtherItemName").GetComponent<Text>();
        Text itemTypeTmp = GameObject.Find("OtherItemArmorDamage").GetComponent<Text>();
        Text itemStatTmp = GameObject.Find("OtherItemArmorDamageStat").GetComponent<Text>();
        itemDetailPanel = itemDetailTmp;
        itemName = itemNameTmp;
        itemTypeStat = itemTypeTmp;
        itemStat = itemStatTmp;

        // Setting the inspection references.
        Transform inspectionTmp = GameObject.Find("InspectionPoint").GetComponent<Transform>();
        GameObject mainCamTmp = GameObject.Find("Main Camera");
        GameObject inspectionCamTmp = GameObject.Find("InspectionCamera");
        MoveBehaviour moveBehaviourTmp = GameObject.Find("Player").GetComponent<MoveBehaviour>();
        inspectionPoint = inspectionTmp;
        mainCamera = mainCamTmp;
        inspectionCamera = inspectionCamTmp;
        moveBehaviour = moveBehaviourTmp;
    }

    void Start()
    {
        itemActions.SetActive(false);
        itemDetailPanel.SetActive(false);
        inspectionCamera.SetActive(false);
    }

    void Update()
    {
        if (isInspecting)
        {
            RotateItem();
        }
    }

    public void OpenItemAction(ItemData item, bool isMerchant)
    {
        selectedItem = item;
        isMerchantGlobal = isMerchant;
        if (currentItemAction != null)
            currentItemAction = null;

        currentItemAction = this;

        if (ItemActionSystem.instance.isActive)
            ItemActionSystem.instance.CloseItemAction();

        if (item == null)
            return;

        if (takeItem != null)
            takeItem.onClick.RemoveAllListeners();
        if (equipItem != null)
            equipItem.onClick.RemoveAllListeners();
        if (buyItem != null)
            buyItem.onClick.RemoveAllListeners();
        if (inspectItem != null)
            inspectItem.onClick.RemoveAllListeners();

        // Activate the appropriate buttons based on inventory type
        takeItem.gameObject.SetActive(!isMerchant);
        equipItem.gameObject.SetActive(!isMerchant && item.itemType == ItemType.Equipment);
        buyItem.gameObject.SetActive(isMerchant);
        inspectItem.gameObject.SetActive(true);

        // Add the correct listeners.
        if (takeItem != null)
            takeItem.onClick.AddListener(TakeItem);
        if (equipItem != null)
            equipItem.onClick.AddListener(EquipItem);
        if (buyItem != null)
            buyItem.onClick.AddListener(BuyItem);
        if (inspectItem != null)
            inspectItem.onClick.AddListener(InspectItem);

        itemActions.SetActive(true);
        OpenItemDetail();
    }

    // Close the action panel.
    public void CloseItemAction()
    {
        itemActions.SetActive(false);
        if (!isInspecting)
        {
            selectedItem = null;
            currentItemAction = null;
        }
        CloseItemDetail();
    }

    // Functions for displaying item details.
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

    // Functions of the item action panel.
    // Function called by the takeItem button.
    public void TakeItem()
    {
        if (selectedItem == null || OtherInventory.currentInventory == null)
            return;
        Inventory.instance.AddItem(selectedItem);
        OtherInventory.currentInventory.RemoveItem(selectedItem);
        CloseItemAction();
    }

    public void EquipItem()
    {
        if (selectedItem == null || OtherInventory.currentInventory == null)
            return;

        ItemActionSystem.instance.SetSelectedItem(selectedItem);
        Inventory.instance.AddItem(selectedItem);
        Inventory.instance.GetEquipment().EquipItem();
        OtherInventory.currentInventory.RemoveItem(selectedItem);

        CloseItemAction();
    }

    // Function called by the buyItem button.
    public void BuyItem()
    {
        Debug.Log("Buying system not implemented yet.");
    }

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
                bounds.Encapsulate(rend.bounds);  // Extend bounds to encapsulate the whole item.

            // Shift the item to center its pivot.
            Vector3 centerOffset = bounds.center - item.transform.position;
            item.transform.position -= centerOffset;
        }

        Rigidbody rb = item.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        Inventory.instance.SendMessage("CloseInventory");
        OtherInventory.currentInventory.SendMessage("CloseInventory");
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
        OpenItemAction(selectedItem, isMerchantGlobal);
        // Enable players' movement.
        moveBehaviour.CanMove();
        Inventory.instance.CanOpen();
        Inventory.instance.SendMessage("OpenInventory");
        OtherInventory.currentInventory.SendMessage("OpenInventory");
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
