/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerController : MonoBehaviour {

    Animation animations;
    Rigidbody rb; // Ajout du Rigidbody

    public float walkSpeed = 100; //5f; 
    public float runSpeed = 10f;
    public float turnSpeed = 150f;
    public float jumpForce = 5f;

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
        
    }
}*/

using UnityEngine;

public class TigerAnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();  // Récupère l'Animator attaché au tigre
    }

    void Update()
    {
        // Animation d'Idle (repos) par défaut
        if (Input.GetKey(KeyCode.I))
        {
            animator.Play("Tiger_001_idle");
        }

        // Animation Idle Rare (une variation de Idle)
        if (Input.GetKey(KeyCode.O))
        {
            animator.Play("Tiger_001_idle_rare");
        }

        // Animation de marche
        if (Input.GetKey(KeyCode.W))
        {
            animator.Play("Tiger_001_walk");
        }

        // Animation de course
        if (Input.GetKey(KeyCode.R))
        {
            animator.Play("Tiger_001_run");
        }
    }
}

