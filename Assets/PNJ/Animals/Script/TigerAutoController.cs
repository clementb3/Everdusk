using UnityEngine;
using System.Collections;

public class TigerAutoController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    public float walkSpeed = 2f; // Vitesse de déplacement
    public float rotationSpeed = 45f; // Réduire la vitesse de rotation
    public float decelerationTime = 1f; // Temps pour ralentir
    public float turnDuration = 2f; // Durée de rotation en douceur

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        StartCoroutine(AutoCycle());
    }

    IEnumerator AutoCycle()
    {
        while (true)
        {
            // 1. Rester en Idle (durée exacte de l'animation)
            animator.SetFloat("State", 0);
            yield return new WaitForSeconds(GetAnimationLength("Tiger_001_idle"));

            // 2. Passer à Idle Rare
            animator.SetFloat("State", 1);
            yield return new WaitForSeconds(GetAnimationLength("Tiger_001_idle_rare"));

            // 3. Préparer la marche
            animator.SetFloat("State", 2);
            yield return new WaitForSeconds(0.2f); 

            // 4. Vérifier que l’animation est bien active
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Tiger_001_walk"))
            {
                yield return null; 
            }

            // 5. Avancer pendant toute la durée de l'animation
            float walkDuration = GetAnimationLength("Tiger_001_walk");
            float elapsedTime = 0f;
            while (elapsedTime < walkDuration)
            {
                rb.linearVelocity = transform.forward * walkSpeed;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 6. Décélération progressive avant l'arrêt
            yield return StartCoroutine(Decelerate());

            // 7. Choisir une nouvelle direction avec une rotation progressive en avançant
            float randomRotation = Random.Range(-90f, 90f); // Moins extrême pour éviter les virages trop brusques
            yield return StartCoroutine(SmoothTurn(randomRotation));
        }
    }

    float GetAnimationLength(string animationName)
    {
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        foreach (AnimationClip clip in ac.animationClips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }
        return 1f; 
    }

    // 🛑 Fonction de décélération progressive
    IEnumerator Decelerate()
    {
        float startSpeed = rb.linearVelocity.magnitude;
        float elapsed = 0f;

        while (elapsed < decelerationTime)
        {
            float newSpeed = Mathf.Lerp(startSpeed, 0, elapsed / decelerationTime);
            rb.linearVelocity = transform.forward * newSpeed;
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector3.zero;
    }

    // 🔄 Fonction pour une rotation fluide en avançant légèrement
    IEnumerator SmoothTurn(float angle)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + angle, 0);
        float elapsed = 0f;
        
        // Activer une animation Idle pendant la rotation
        animator.SetFloat("State", 2);

        while (elapsed < turnDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / turnDuration);
            rb.linearVelocity = transform.forward * (walkSpeed * 0.5f); // Déplacement léger vers l'avant pour lisser la rotation
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        rb.linearVelocity = Vector3.zero; // Arrêter après la rotation
    }
}
