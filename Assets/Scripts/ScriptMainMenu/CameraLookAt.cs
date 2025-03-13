using UnityEngine;

/* public class CameraLookAt : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        if (target != null)
            transform.LookAt(target);
    }
} */

public class CameraLookAt : MonoBehaviour
{
    public Transform objectToInspect;
    public float rotationSpeed = 100f;
    private Vector3 previousMousePosition;
    void Update()
    {
        if ( Input.GetMouseButtonDown (0) )
        {
            previousMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton (0) )
        {
            Vector3 deltaMousePosition = Input.mousePosition - previousMousePosition;
            float rotationY = -deltaMousePosition.x * rotationSpeed * Time. deltaTime;
            
            Quaternion rotation = Quaternion.Euler(0, rotationY, 0) ;
            objectToInspect.rotation = rotation * objectToInspect.rotation;
            
            previousMousePosition = Input.mousePosition;
        }
    }
}