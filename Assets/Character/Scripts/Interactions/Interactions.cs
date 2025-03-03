using UnityEngine;

public class Interactions : MonoBehaviour
{
    [Header("PARAMETERS")]
    [SerializeField]
    private float range = 1.2f;                                     // Range to pick up an item.
    [SerializeField]
    private float detectionAngle;                                   // Max angle to detect an object.
    [Header("DEBUG PARAMETERS")]
    [SerializeField]
    private int segments;                                           // Debug variable to see the detection cone.
    [SerializeField]
    private int segmentsCapsule;                                           // Debug variable to see the detection cone.

    [Header("REFERENCES")]
    public InteractionsBehaviour interactionsBehaviour;             // Reference to the behaviour to have in a situation.
    public Transform player;                                        // Player's reference.
    private bool chestOpen = false;                                 // Boolean to check if the player is looking into a chest.

    void Update()
    {
        // Start of the cylinder (feet of the player).
        Vector3 start = player.position;
        // End of the cylinder (head of the player).
        Vector3 end = player.position + Vector3.up * 1.9f;

        DebugDrawCone(player.position, player.forward, detectionAngle, range, segments, Color.yellow);
        DebugDrawCapsule(start, end, range, Color.blue, segmentsCapsule);

        // Reference the objects in the detection sphere arround the player.
        Collider[] detectedObjects = Physics.OverlapCapsule(start, end, range);

        // Loop on each detected items.
        foreach (Collider col in detectedObjects)
        {
            if (IsInFront(col, start))
            {
                // If the object is in the cone...
                if (col.CompareTag("Item"))
                {
                    // If it's an item...
                    Debug.Log("Item detected : " + col.name);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // Pick up the item.
                        interactionsBehaviour.Pickup(col.GetComponent<Item>());
                        break;
                    }
                }
                if (col.CompareTag("Chest"))
                {
                    // If it's a chest...
                    Debug.Log("Chest detected : " + col.name);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (chestOpen)
                        {
                            // If a chest is already opened : close it.
                            interactionsBehaviour.CloseChest(col);
                            chestOpen = false;
                            break;
                        }
                        else
                        {
                            // Else : open it.
                            interactionsBehaviour.OpenChest(col);
                            chestOpen = true;
                            break;
                        }
                    }
                }
            }
        }
    }

    bool IsInFront(Collider item, Vector3 start)
    {
        // Check if it's in the cone in front of the player.
        Vector3 directionToItem = (item.transform.position - start).normalized;
        Vector3 horizontalDirToItem = new Vector3(directionToItem.x, 0, directionToItem.z).normalized;
        float angle = Vector3.Angle(new Vector3(player.forward.x, 0, player.forward.z), horizontalDirToItem);
        return angle < detectionAngle;
    }

    // Debug functions for the collision capsule.
    // Method to draw the detection cone.
    void DebugDrawCone(Vector3 origin, Vector3 forward, float angle, float range, int segments, Color color)
    {
        float halfAngle = angle * 0.5f;
        Vector3 flatForward = new Vector3(forward.x, 0, forward.z).normalized;  
        for (int i = 0; i <= segments; i++)
        {
            float step = Mathf.Lerp(-halfAngle, halfAngle, i / (float)segments);
            Quaternion rotation = Quaternion.AngleAxis(step, Vector3.up); 
            Vector3 direction = rotation * flatForward;
            Debug.DrawRay(origin, direction * range, color);
        }
        Debug.DrawRay(origin, flatForward * range, Color.red);
    }


    // Methods to draw the detection cylinder.
    void DebugDrawCapsule(Vector3 start, Vector3 end, float radius, Color color, int segments)
    {
        DebugDrawCircle(start, radius, color, segments);
        DebugDrawCircle(end, radius, color, segments);

        // Link both circles to simulate a cylinder.
        for (int i = 0; i < segments; i++)
        {
            float angle = i / (float)segments * 2 * Mathf.PI;
            float nextAngle = (i + 1) / (float)segments * 2 * Mathf.PI;

            Vector3 offset1 = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            Vector3 offset2 = new Vector3(Mathf.Cos(nextAngle) * radius, 0, Mathf.Sin(nextAngle) * radius);

            Debug.DrawLine(start + offset1, start + offset2, color);
            Debug.DrawLine(end + offset1, end + offset2, color);
            Debug.DrawLine(start + offset1, end + offset1, color);
        }
    }

    void DebugDrawCircle(Vector3 center, float radius, Color color, int segments)
    {
        Vector3 prevPoint = center + new Vector3(radius, 0, 0);
        for (int i = 1; i <= segments; i++)
        {
            float angle = i / (float)segments * 2 * Mathf.PI;
            Vector3 newPoint = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            Debug.DrawLine(prevPoint, newPoint, color);
            prevPoint = newPoint;
        }
    }
}
