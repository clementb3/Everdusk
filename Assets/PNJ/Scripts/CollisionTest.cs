using UnityEngine;

public class CollisionTest : MonoBehaviour {
    void OnCollisionEnter(Collision collision) {
        Debug.Log("Collision avec : " + collision.gameObject.name);
    }
}