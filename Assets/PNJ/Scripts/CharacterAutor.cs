/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAutor : MonoBehaviour {

    Animation animations;
    
    public float walkSpeed;
    public float runSpeed;
    public float turnSpeed;

    public string inputFront;
    public string inputBack;
    public string inputLeft;
    public string inputRight;    

    public Vector3 jumpSpeed;
    CapsuleCollider playerCollider;

    void Start() {
        animations = gameObject.GetComponent<Animation>();
        playerCollider = gameObject.GetComponent<CapsuleCollider>();
    }

    void Update() {
        if(Input.GetKey(inputFront)){
            transform.Translate(0, 0, walkSpeed * Time.deltaTime);
            animations.Play("walk");
        }
        if(Input.GetKey(inputBack)){
            transform.Translate(0, 0, -(walkSpeed/2) * Time.deltaTime);
            animations.Play("walk");
        }
        if(Input.GetKey(inputLeft)){
            transform.Rotate(0, -turnSpeed * Time.deltaTime, 0);
        }
        if(Input.GetKey(inputRight)){
            transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
        }

    }
}*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAutor : MonoBehaviour {

    Animation animations;
    Rigidbody rb; // Ajout du Rigidbody

    public float walkSpeed = 100; //5f; 
    public float runSpeed = 10f;
    public float turnSpeed = 150f;
    public float jumpForce = 5f;

    public string inputFront = "z";
    public string inputBack = "s";
    public string inputLeft = "q";
    public string inputRight = "d";    

    public Vector3 jumpSpeed;
    CapsuleCollider playerCollider;

    void Start() {
        animations = GetComponent<Animation>();
        playerCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        // Assurer que Rigidbody n'est pas kinematic pour détecter les collisions
        rb.isKinematic = false; 
        rb.freezeRotation = true; 
    }

    void FixedUpdate() { // Utiliser FixedUpdate pour la physique
        Vector3 moveDirection = Vector3.zero;

        if(Input.GetKey(inputFront)){
            moveDirection += transform.forward;
            animations.Play("walk");
        }
        if(Input.GetKey(inputBack)){
            moveDirection -= transform.forward;
            animations.Play("walk");
        } 
        if(Input.GetKey(inputLeft)){
            transform.Rotate(0, -turnSpeed * Time.fixedDeltaTime, 0);
        }
        if(Input.GetKey(inputRight)){
            transform.Rotate(0, turnSpeed * Time.fixedDeltaTime, 0);
        }

        // Appliquer la force pour le déplacement (au lieu de MovePosition)
        rb.linearVelocity = new Vector3(moveDirection.x * walkSpeed, rb.linearVelocity.y, moveDirection.z * walkSpeed);

        // Saut (ajouté pour tester la physique)
        if (Input.GetKeyDown(KeyCode.Space)) {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
