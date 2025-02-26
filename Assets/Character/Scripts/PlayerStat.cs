using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    [Header("HEALTH")]
    [SerializeField]
    private float maxHealth = 100f;              // References the maximum health points of the player.
    [SerializeField]
    private float currentHealth;                 // References the current health points of the player.
    [SerializeField]
    private Image healthImage;                   // References the health image in the canvas.
    public float currentArmor;

    [Header("MANA")]
    [SerializeField]
    private float maxMana = 100f;                // References the maximum mana points of the player.
    private float currentMana;                   // References the current mana points of the player.
    private float refillMana = 1f;               // References the number of mana point to refill the current mana points of the player.
    [SerializeField]
    private Image manaImage;                     // References the mana image in the canvas.

    [Header("OTHER REFERENCES")]
    [SerializeField]
    private MoveBehaviour moveBehaviour;         // Management of the players' movement.
    [SerializeField]
    private Animator animator;                   // Management of the different animations.
    [SerializeField]
    private Attack attack;                       // Management of the attack script.
    private bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
    }

    void Update()
    {
        if (currentMana < maxMana)
            RefillMana();
        if (Input.GetKeyDown(KeyCode.K))
            TakeDamage(10);
        if (Input.GetKeyDown(KeyCode.M))
            LossMana(10);
    }

    // Inflict damage to the player
    public void TakeDamage(float damage)
    {
        // Remove the armor points to the damage inflicted
        currentHealth -= damage - currentArmor;
        // Plays the death animation if players' health <= 0
        if (currentHealth <= 0 && !isDead)
            Die();
        UpdateHealthBar();
    }

    void Die()
    {
        isDead = true;
        // Disable the players' movement, aiming and attack script.
        moveBehaviour.CanMove();
        attack.enabled = false;
        // Play the death animation.
        animator.SetTrigger("Dead");
    }

    // Update the visuals of health and mana bars.
    void UpdateHealthBar()
    {
        healthImage.fillAmount = currentHealth / maxHealth;
    }

    // Remove the amount of mana needed for a spell.
    void LossMana(float mana)
    {
        currentMana -= mana;
        UpdateManaBar();
    }

    void UpdateManaBar()
    {
        manaImage.fillAmount = currentMana / maxMana;
    }

    // Refill over time the mana bar.
    void RefillMana()
    {
        currentMana += refillMana * Time.deltaTime;
        UpdateManaBar();
    }

    // Getter.
    public bool IsDead()
    {
        return isDead;
    }
}
