using UnityEngine;

// MoveBehaviour inherits from GenericBehaviour. This class corresponds to basic walk and run behaviour, it is the default behaviour.
public class MoveBehaviour : GenericBehaviour
{
	private float speed, speedSeeker;               // Moving speed.
	private bool jump;                              // Boolean to determine whether or not the player started a jump.
	private bool isColliding;                       // Boolean to determine if the player has collided with an obstacle.
	private bool canMove = true;                    // Boolean to determine if the player can move or is doing an animation.
	private AnimationManager animationManager;
	void Start()
	{
		// Set up the references.
		animationManager = new AnimationManager(behaviourManager.GetAnim);
		animationManager.SetGrounded(true);

		// Subscribe and register this behaviour as the default behaviour.
		behaviourManager.SubscribeBehaviour(this);
		behaviourManager.RegisterDefaultBehaviour(behaviourCode);
		speedSeeker = GlobalSettings.runSpeed*300;
	}

	// Update is used to set features regardless the active behaviour.
	void Update()
	{
		// Get jump input.
		if (!jump && InputManager.IsJumping() && behaviourManager.IsCurrentBehaviour(behaviourCode) && !behaviourManager.IsOverriding())
		{
			jump = true;
		}
	}

	// LocalFixedUpdate overrides the virtual function of the base class.
	public override void LocalFixedUpdate()
	{
		// Call the basic movement manager.
		Movement(behaviourManager.GetH, behaviourManager.GetV);
		// Call the jump manager.
		Jump();
	}

	// Execute the idle and walk/run jump movements.
	void Jump()
	{
		if (!canMove)
			return;

		float initialSpeed = animationManager.GetSpeed();
		// Start a new jump.
		if (jump && !animationManager.IsJumping() && behaviourManager.IsGrounded())
		{
			// Set jump related parameters.
			behaviourManager.LockTempBehaviour(behaviourCode);
			animationManager.SetJump(true);

			// Temporarily change player friction to pass through obstacles.
			GetComponent<CapsuleCollider>().material.dynamicFriction = 0f;
			GetComponent<CapsuleCollider>().material.staticFriction = 0f;

			// Remove vertical velocity to avoid "super jumps".
			RemoveVerticalVelocity();

			// Set jump vertical impulse velocity.
			float velocity = 2f * Mathf.Abs(Physics.gravity.y) * GlobalSettings.jumpHeight;
			velocity = Mathf.Sqrt(velocity);
			behaviourManager.GetRigidBody.AddForce(Vector3.up * velocity, ForceMode.VelocityChange);
		}
		// Is already jumping?
		else if (animationManager.IsJumping())
		{
			// Keep forward movement while in the air if the player was already moving.
			if (!behaviourManager.IsGrounded() && !isColliding && behaviourManager.GetTempLockStatus() && initialSpeed > 0.1)
			{
				// Add of an horizontal force.
				behaviourManager.GetRigidBody.AddForce(GlobalSettings.jumpHorizontalForce * Physics.gravity.magnitude * GlobalSettings.sprintSpeed * transform.forward, ForceMode.Acceleration);
			}
			// Has landed?
			if ((behaviourManager.GetRigidBody.linearVelocity.y < 0) && behaviourManager.IsGrounded())
			{
				animationManager.SetGrounded(true);
				// Change back player friction to default.
				GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f;
				GetComponent<CapsuleCollider>().material.staticFriction = 0.6f;
				// Set jump related parameters.
				jump = false;
				animationManager.SetJump(false);
				behaviourManager.UnlockTempBehaviour(behaviourCode);
			}
		}
	}

	// Deal with the basic player movement
	void Movement(float horizontal, float vertical)
	{
		// Disable movemet if the player cannot move (e.g. playing a special animation)
		if (!canMove)
			return;

		// On ground, obey gravity.
		if (behaviourManager.IsGrounded())
			behaviourManager.GetRigidBody.useGravity = true;

		// Avoid takeoff when reached a slope end.
		else if (!animationManager.IsJumping() && behaviourManager.GetRigidBody.linearVelocity.y > 0)
		{
			RemoveVerticalVelocity();
		}

		// Call function that deals with player orientation.
		Rotating(horizontal, vertical);

		// Set proper speed.
		Vector2 dir = new(horizontal, vertical);
		speed = Vector2.ClampMagnitude(dir, 1f).magnitude;
		speedSeeker += InputManager.GetScrollWheelAxis();
		speedSeeker = Mathf.Clamp(speedSeeker, GlobalSettings.walkSpeed, GlobalSettings.runSpeed);
		speed *= speedSeeker;
		if (behaviourManager.IsSprinting())
		{
			speed = GlobalSettings.sprintSpeed;
		}

		animationManager.SetSpeed(speed, GlobalSettings.speedDampTime, Time.deltaTime);
	}

	public void StopMovement()
	{
		// Stop the player.
		behaviourManager.GetRigidBody.linearVelocity = Vector3.zero;
		// Stop players' rotation.
		behaviourManager.GetRigidBody.angularVelocity = Vector3.zero;
		// Stop the players' animation (if moving).
		animationManager.SetSpeed(0f, 0f, 0f);
	}

	// Remove vertical rigidbody velocity.
	private void RemoveVerticalVelocity()
	{
		Vector3 horizontalVelocity = behaviourManager.GetRigidBody.linearVelocity;
		horizontalVelocity.y = 0;
		behaviourManager.GetRigidBody.linearVelocity = horizontalVelocity;
	}

	// Rotate the player to match correct orientation, according to camera and key pressed.
	Vector3 Rotating(float horizontal, float vertical)
	{
		// Get camera forward direction, without vertical component.
		Vector3 forward = behaviourManager.playerCamera.TransformDirection(Vector3.forward);

		// Player is moving on ground, Y component of camera facing is not relevant.
		forward.y = 0.0f;
		forward = forward.normalized;

		// Calculate target direction based on camera forward and direction key.
		Vector3 right = new(forward.z, 0, -forward.x);
		Vector3 targetDirection;
		targetDirection = forward * vertical + right * horizontal;

		// Lerp current direction to calculated target direction.
		if (behaviourManager.IsMoving() && targetDirection != Vector3.zero)
		{
			Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

			Quaternion newRotation = Quaternion.Slerp(behaviourManager.GetRigidBody.rotation, targetRotation, GlobalSettings.turnSmoothing);
			behaviourManager.GetRigidBody.MoveRotation(newRotation);
			behaviourManager.SetLastDirection(targetDirection);
		}
		// If idle, Ignore current camera facing and consider last moving direction.
		if (!(Mathf.Abs(horizontal) > 0.9 || Mathf.Abs(vertical) > 0.9))
		{
			behaviourManager.Repositioning();
		}

		return targetDirection;
	}

	// Enable/disable players' movement.
	public void CanMove()
	{
		canMove = !canMove;
	}

	// Getter.
	public bool GetCanMove()
	{
		return canMove;
	}
}
