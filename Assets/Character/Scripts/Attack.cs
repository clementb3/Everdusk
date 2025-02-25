using System.Linq;
using UnityEngine;

public class Attack : GenericBehaviour
{
    private bool isAttacking;
    private AnimationManager animationManager;   // Management of the different animations.
    [SerializeField]
    private Equipment equipment;                 // Reference to the equipment class.
    [SerializeField]
    private GameObject[] UIPanels;               // References to different panels like the inventory.
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private Vector3 attackOffset;
    void Start()
    {
        animationManager = new AnimationManager(behaviourManager.GetAnim);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position + attackOffset, transform.forward * attackRange, Color.red);
        if (UIPanels.Any((panel) => panel == panel.activeSelf))
            return;
        if (InputManager.IsAttacking() && !isAttacking)
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
                Debug.Log("enemy hit");
        }
    }

    public void AttackFinished()
    {
        isAttacking = false;
    }
}
