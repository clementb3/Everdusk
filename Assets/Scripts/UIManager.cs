using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] UIPanels;     // References to different panels like the inventory.
    private float defaultHSpeed;       // Reference to the default horizontal aiming speed stored in GlobalSettings.
    private float defaultVSpeed;       // Reference to the default vertical aiming speed stored in GlobalSettings.
    [HideInInspector]
    public bool panelOpened = false;   // Bool to check if a panel is opened.
    void Start()
    {
        defaultHSpeed = GlobalSettings.horizontalAimingSpeed;
        defaultVSpeed = GlobalSettings.verticalAimingSpeed;
    }

    void Update()
    {
        panelOpened = UIPanels.Any((panel) => panel == panel.activeSelf);
        if (panelOpened)
        {
            // Disable camera movement.
            GlobalSettings.horizontalAimingSpeed = 0f;
            GlobalSettings.verticalAimingSpeed = 0f;
        }
        else
        {
            // Enable camera movement.
            GlobalSettings.horizontalAimingSpeed = defaultHSpeed;
            GlobalSettings.verticalAimingSpeed = defaultVSpeed;
        }
    }
}
