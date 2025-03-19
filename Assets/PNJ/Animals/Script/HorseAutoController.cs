using UnityEngine;
using System.Collections;

public class HorseAutoController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    public float walkSpeed = 2f; // Vitesse de déplacement
    public float rotationSpeed = 45f; // Réduire la vitesse de rotation
    public float decelerationTime = 0.5f; // Temps pour ralentir
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
            animator.SetFloat("State", 0); // Idle
            yield return new WaitForSeconds(GetAnimationLength("Horse_001_idle"));

            // 2. Passer à Walk (marche)
            animator.SetFloat("State", 1); // Walk
            yield return new WaitForSeconds(0.2f); // Laisser le temps à l'animation de démarrer

            // 3. Vérifier que l’animation de marche est bien active
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Horse_001_walk"))
            {
                yield return null; // Attendre que l'animation "walk" commence
            }

            // 4. Avancer pendant toute la durée de l'animation Walk
            float walkDuration = GetAnimationLength("Horse_001_walk");
            float elapsedTime = 0f;
            while (elapsedTime < walkDuration + 2)
            {
                rb.linearVelocity = transform.forward * walkSpeed; // Déplacement avec Rigidbody
                elapsedTime += Time.deltaTime;
                yield return null; // Avancer à chaque frame
            }

            // 5. Décélération progressive avant l'arrêt
            yield return StartCoroutine(Decelerate());

            // 6. Choisir une nouvelle direction avec une rotation progressive en avançant
            float randomRotation = Random.Range(-90f, 90f); // Moins extrême pour éviter les virages trop brusques
            yield return StartCoroutine(SmoothTurn(randomRotation));

            // 7. Passer à Eat (manger)
            animator.SetFloat("State", 2); // Eat
            yield return new WaitForSeconds(GetAnimationLength("Horse_001_eat"));
        }
    }

    float GetAnimationLength(string animationName)
    {
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        foreach (AnimationClip clip in ac.animationClips)
        {
            if (clip.name == animationName)
            {
                return clip.length; // Retourne la durée exacte de l'animation
            }
        }
        return 1f; // Valeur par défaut si l'animation n'est pas trouvée
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
        animator.SetFloat("State", 0);

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
