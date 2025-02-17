using UnityEngine;

public class Attack : GenericBehaviour
{
    private bool isAttacking;
    // Management of the different animations.
    private AnimationManager animationManager;
    void Start()
    {
        animationManager = new AnimationManager(behaviourManager.GetAnim);
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.IsAttacking() && !isAttacking)
        {
            isAttacking = true; 
            animationManager.Attack();
        }
    }

    public void AttackFinished()
    {
        isAttacking = false;
    }
}
