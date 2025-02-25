using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] UIPanels;     // References to different panels like the inventory.
    private float defaultHSpeed;       // Reference to the default horizontal aiming speed stored in GlobalSettings.
    private float defaultVSpeed;       // Reference to the default vertical aiming speed stored in GlobalSettings.
    void Start()
    {
        defaultHSpeed = GlobalSettings.horizontalAimingSpeed;
        defaultVSpeed = GlobalSettings.verticalAimingSpeed;
    }

    void Update()
    {
        if(UIPanels.Any((panel) => panel == panel.activeSelf))
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
