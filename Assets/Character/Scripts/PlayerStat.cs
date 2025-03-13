using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    [Header("HEALTH")]
    [SerializeField]
    private float maxHealth = 100f;    // References the maximum health points of the player.
    private float currentHealth;       // References the current health points of the player.
    [SerializeField]
    private Image healthImage;         // References the health image in the canvas.

    [Header("MANA")]
    [SerializeField]
    private float maxMana = 100f;      // References the maximum mana points of the player.
    private float currentMana;         // References the current mana points of the player.
    private float refillMana = 1f;          // References the number of mana point to refill the current mana points of the player.
    [SerializeField]
    private Image manaImage;           // References the mana image in the canvas.

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

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Debug.Log("Player died");
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        healthImage.fillAmount = currentHealth / maxHealth;
    }

    void LossMana(float mana)
    {
        currentMana -= mana;
        UpdateManaBar();
    }

    void UpdateManaBar()
    {
        manaImage.fillAmount = currentMana / maxMana;
    }

    void RefillMana()
    {
        currentMana += refillMana * Time.deltaTime;
        UpdateManaBar();
    }
}
