using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private float range = 1.2f;                                                                    // Range to pick up an item.
    public PickupBehaviour pickupBehaviour;                                                        // Reference to the inventory.
    public Transform player;                                                                       // Player's reference.

    void Update()
    {
        Vector3 start = player.position;                                                           // Start of the cylinder (feet of the player).
        Vector3 end = player.position + Vector3.up * 1.9f;                                         // End of the cylinder (head of the player).

        float detectionAngle = 45f;                                                                // Max angle to detect items.

        DebugDrawCone(player.position, player.forward, 45f, range, 10, Color.yellow);
        DebugDrawCapsule(start, end, range, Color.blue);

        Collider[] detectedObjects = Physics.OverlapCapsule(start, end, range);                    // Reference the objects in the detection sphere arround the player.

        foreach (Collider col in detectedObjects)                                                  // Loop on each detected items.
        {
            if (col.CompareTag("Item"))                                                            // If it's an item...
            {
                Vector3 directionToItem = (col.transform.position - start).normalized;             // Check if it's in the cone in front of the player.
                
                Vector3 horizontalDirToItem = new Vector3(directionToItem.x, 0, directionToItem.z).normalized;
                float angle = Vector3.Angle(new Vector3(player.forward.x, 0, player.forward.z), horizontalDirToItem);

                if (angle < detectionAngle)                                                        // If the item is in the cone...
                {
                    Debug.Log("Item détecté : " + col.name);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        pickupBehaviour.Pickup(col.GetComponent<Item>());                          // Pick up the item.
                        break;
                    }
                }
            }
        }
    }

    // Debug functions for the collision capsule.
    // Method to draw the detection cone.
    void DebugDrawCone(Vector3 origin, Vector3 forward, float angle, float range, int segments, Color color)
    {
        float halfAngle = angle * 0.5f;

        for (int i = 0; i <= segments; i++)
        {
            float step = i / (float)segments * angle - halfAngle;
            Quaternion rotation = Quaternion.Euler(0, step, 0);
            Vector3 direction = rotation * forward;
            Debug.DrawRay(origin, direction * range, color);
        }
    }

    // Methods to draw the detection cylinder.
    void DebugDrawCapsule(Vector3 start, Vector3 end, float radius, Color color)
    {
        int segments = 24;
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
