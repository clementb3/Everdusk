using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

// Class to manage the animations.
public class AnimationManager
{
    private readonly Animator animator;

    // Defining constant hashed parameters
    private readonly int speedHash;
    private readonly int jumpHash;
    private readonly int groundedHash;
    private readonly int aimHash;
    private readonly int h;
    private readonly int v;

    public AnimationManager(Animator animator)
    {
        this.animator = animator;

        // Hash of the different parameters
        speedHash = Animator.StringToHash("Speed");         // Name of the speed var in the Animator.
        jumpHash = Animator.StringToHash("Jump");           // Name of the jump var in the Animator.
        groundedHash = Animator.StringToHash("Grounded");   // Name of the grounded var in the Animator.
        aimHash = Animator.StringToHash("Aim");             // Name of the aim var in the Animator.
        h = Animator.StringToHash("H");                     // Name of the horizontal axis var in the Animator.
        v = Animator.StringToHash("V");                     // Name of the vertical axis var in the Animator.
    }

    // Movement
    public void SetSpeed(float speed, float speedDampTime = 0, float deltaTime = 0)
    {
        animator.SetFloat(speedHash, speed, speedDampTime, deltaTime);
    }

    public float GetSpeed()
    {
        return animator.GetFloat(speedHash);
    }

    // Jumping
    public void SetJump(bool isJumping)
    {
        animator.SetBool(jumpHash, isJumping);
    }

    public bool IsJumping()
    {
        return animator.GetBool(jumpHash);
    }

    public void SetGrounded(bool isGrounded)
    {
        animator.SetBool(groundedHash, isGrounded);
    }

    // Aiming
    public void SetAim(bool isAiming)
    {
        animator.SetBool(aimHash, isAiming);
    }

    // Axis
    public void SetHorizontal(float horizontal)
    {
        animator.SetFloat(h,horizontal, 0.1f, Time.deltaTime);
    }

    public void SetVertical(float vertical)
    {
        animator.SetFloat(v, vertical, 0.1f, Time.deltaTime);
    }
}
