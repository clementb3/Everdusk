using System.Linq;
using UnityEngine;

public class Attack : GenericBehaviour
{
    private bool isAttacking;
    private AnimationManager animationManager;   // Management of the different animations.
    [Header("SCRIPT REFERENCES")]
    [SerializeField]
    private Equipment equipment;                 // Reference to the equipment class.
    [SerializeField]
    private UIManager uIManager;                 // References to different panels like the inventory.
    [SerializeField]
    private InteractionsBehaviour interactionsBehaviour;

    [Header("PARAMETERS")]
    [SerializeField]
    private float attackRange;                   // Range at which the player hit the enemy.
    [SerializeField]
    private Vector3 attackOffset;                // Offset to put the 
    void Start()
    {
        animationManager = new AnimationManager(behaviourManager.GetAnim);
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.IsAttacking() && CanAttack())
        {
            isAttacking = true;
            if (equipment.IsWeaponEquipped())
            {
                animationManager.AttackOneHanded();
                SendAttack();
            }
            else
            {
                animationManager.AttackFist();
                SendAttack();
            }
        }
    }

    public void SendAttack()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position + attackOffset, transform.forward, out hit, attackRange))
        {
            if(hit.transform.CompareTag("Enemy"))
            {
                EnemyAI enemy = hit.transform.GetComponent<EnemyAI>();
                // If we are using our fists.
                if(!equipment.GetEquippedWeapon())
                    enemy.TakeDamage(2f);
                else
                    enemy.TakeDamage(equipment.GetEquippedWeapon().weaponDamage);
            }
        }
    }

    // Check if th player is not doing another action or if the inventory is not open.
    bool CanAttack()
    {
        return !isAttacking && !uIManager.panelOpened && !interactionsBehaviour.isBusy;
    }
    public void AttackFinished()
    {
        isAttacking = false;
    }
}
