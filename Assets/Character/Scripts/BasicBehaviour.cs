﻿using UnityEngine;
using System.Collections.Generic;

// This class manages which player behaviour is active or overriding, and call its local functions.
public class BasicBehaviour : MonoBehaviour
{
	public Transform playerCamera;                        // Reference to the camera that focus the player.
	private float h;                                      // Horizontal Axis.
	private float v;                                      // Vertical Axis.
	private int currentBehaviour;                         // Reference to the current player behaviour.
	private int defaultBehaviour;                         // The default behaviour of the player when any other is not active.
	private int behaviourLocked;                          // Reference to temporary locked behaviour that forbids override.
	private Vector3 lastDirection;                        // Last direction the player was moving.
	private Animator anim;                                // Reference to the Animator component.
	private ThirdPersonOrbitCamBasic camScript;           // Reference to the third person camera script.
	private bool sprint;                                  // Boolean to determine whether or not the player activated the sprint mode.
	private List<GenericBehaviour> behaviours;            // The list containing all the enabled player behaviours.
	private List<GenericBehaviour> overridingBehaviours;  // List of current overriding behaviours.
	private Rigidbody rBody;                              // Reference to the player's rigidbody.
    private Vector3 colExtents;                           // Collider extents for ground test. 
	private AnimationManager animationManager;            // Management of the different animations.

	// Get current horizontal and vertical axes.
	public float GetH { get { return h; } }
	public float GetV { get { return v; } }

	// Get the player camera script.
	public ThirdPersonOrbitCamBasic GetCamScript { get { return camScript; } }

	// Get the player's rigid body.
	public Rigidbody GetRigidBody { get { return rBody; } }

	// Get the player's animator controller.
	public Animator GetAnim { get { return anim; } }

	// Get current default behaviour.
	public int GetDefaultBehaviour {  get { return defaultBehaviour; } }

	void Awake ()
	{
		// References to the actions of the player
		behaviours = new List<GenericBehaviour> ();
		overridingBehaviours = new List<GenericBehaviour>();
		// References to the Animator
		anim = GetComponent<Animator> ();
		animationManager = new AnimationManager(anim);
		// References to the camera
		camScript = playerCamera.GetComponent<ThirdPersonOrbitCamBasic> ();
		// References to the player body
		rBody = GetComponent<Rigidbody> ();

		// Grounded verification variables.
		colExtents = GetComponent<Collider>().bounds.extents;
	}

	void Update()
	{
		// Store the input axes.
		h = InputManager.GetHorizontalAxis();
		v = InputManager.GetVerticalAxis();                  

		// Set the input axes on the Animator Controller.
		animationManager.SetHorizontal(h);
		animationManager.SetVertical(v);

		// Toggle sprint by input.
		sprint = InputManager.IsSprinting();

        // Set the grounded test on the Animator Controller.
        animationManager.SetGrounded(IsGrounded());
	}

	// Call the FixedUpdate functions of the active or overriding behaviours.
	void FixedUpdate()
	{
		// Call the active behaviour if no other is overriding.
		bool isAnyBehaviourActive = false;
		if (behaviourLocked > 0 || overridingBehaviours.Count == 0)
		{
			foreach (GenericBehaviour behaviour in behaviours)
			{
				if (behaviour.isActiveAndEnabled && currentBehaviour == behaviour.GetBehaviourCode())
				{
					isAnyBehaviourActive = true;
					behaviour.LocalFixedUpdate();
				}
			}
		}
		// Call the overriding behaviours if any.
		else
		{
			foreach (GenericBehaviour behaviour in overridingBehaviours)
			{
				behaviour.LocalFixedUpdate();
			}
		}

		// Ensure the player will stand on ground if no behaviour is active or overriding.
		if (!isAnyBehaviourActive && overridingBehaviours.Count == 0)
		{
			rBody.useGravity = true;
			Repositioning ();
		}
	}

	// Call the LateUpdate functions of the active or overriding behaviours.
	private void LateUpdate()
	{
		// Call the active behaviour if no other is overriding.
		if (behaviourLocked > 0 || overridingBehaviours.Count == 0)
		{
			foreach (GenericBehaviour behaviour in behaviours)
			{
				if (behaviour.isActiveAndEnabled && currentBehaviour == behaviour.GetBehaviourCode())
				{
					behaviour.LocalLateUpdate();
				}
			}
		}
		// Call the overriding behaviours if any.
		else
		{
			foreach (GenericBehaviour behaviour in overridingBehaviours)
			{
				behaviour.LocalLateUpdate();
			}
		}

	}

	// Put a new behaviour on the behaviours watch list.
	public void SubscribeBehaviour(GenericBehaviour behaviour)
	{
		behaviours.Add (behaviour);
	}

	// Set the default player behaviour.
	public void RegisterDefaultBehaviour(int behaviourCode)
	{
		defaultBehaviour = behaviourCode;
		currentBehaviour = behaviourCode;
	}

	// Attempt to set a custom behaviour as the active one.
	// Always changes from default behaviour to the passed one.
	public void RegisterBehaviour(int behaviourCode)
	{
		if (currentBehaviour == defaultBehaviour)
		{
			currentBehaviour = behaviourCode;
		}
	}

	// Attempt to deactivate a player behaviour and return to the default one.
	public void UnregisterBehaviour(int behaviourCode)
	{
		if (currentBehaviour == behaviourCode)
		{
			currentBehaviour = defaultBehaviour;
		}
	}

	// Attempt to override any active behaviour with the behaviours on queue.
	// Use to change to one or more behaviours that must overlap the active one (ex.: aim behaviour).
	public bool OverrideWithBehaviour(GenericBehaviour behaviour)
	{
		// Behaviour is not on queue.
		if (!overridingBehaviours.Contains(behaviour))
		{
			// No behaviour is currently being overridden.
			if (overridingBehaviours.Count == 0)
			{
				// Call OnOverride function of the active behaviour before overrides it.
				foreach (GenericBehaviour overriddenBehaviour in behaviours)
				{
					if (overriddenBehaviour.isActiveAndEnabled && currentBehaviour == overriddenBehaviour.GetBehaviourCode())
					{
						overriddenBehaviour.OnOverride();
						break;
					}
				}
			}
			// Add overriding behaviour to the queue.
			overridingBehaviours.Add(behaviour);
			return true;
		}
		return false;
	}

	// Attempt to revoke the overriding behaviour and return to the active one.
	// Called when exiting the overriding behaviour (ex.: stopped aiming).
	public bool RevokeOverridingBehaviour(GenericBehaviour behaviour)
	{
		if (overridingBehaviours.Contains(behaviour))
		{
			overridingBehaviours.Remove(behaviour);
			return true;
		}
		return false;
	}

	// Check if any or a specific behaviour is currently overriding the active one.
	public bool IsOverriding(GenericBehaviour behaviour = null)
	{
		if (behaviour == null)
			return overridingBehaviours.Count > 0;
		return overridingBehaviours.Contains(behaviour);
	}

	// Check if the active behaviour is the passed one.
	public bool IsCurrentBehaviour(int behaviourCode)
	{
		return currentBehaviour == behaviourCode;
	}

	// Check if any other behaviour is temporary locked.
	public bool GetTempLockStatus(int behaviourCodeIgnoreSelf = 0)
	{
		return behaviourLocked != 0 && behaviourLocked != behaviourCodeIgnoreSelf;
	}

	// Atempt to lock on a specific behaviour.
	// No other behaviour can overrhide during the temporary lock.
	// Use for temporary transitions like jumping, entering/exiting aiming mode, etc.
	public void LockTempBehaviour(int behaviourCode)
	{
		if (behaviourLocked == 0)
		{
			behaviourLocked = behaviourCode;
		}
	}

	// Attempt to unlock the current locked behaviour.
	// Use after a temporary transition ends.
	public void UnlockTempBehaviour(int behaviourCode)
	{
		if(behaviourLocked == behaviourCode)
		{
			behaviourLocked = 0;
		}
	}

	// Common functions to any behaviour:
	// Check if player is sprinting.
	public virtual bool IsSprinting()
	{
		return sprint && IsMoving() && CanSprint();
	}

	// Check if player can sprint (all behaviours must allow).
	public bool CanSprint()
	{
		foreach (GenericBehaviour behaviour in behaviours)
		{
			if (!behaviour.AllowSprint ())
				return false;
		}
		foreach(GenericBehaviour behaviour in overridingBehaviours)
		{
			if (!behaviour.AllowSprint())
				return false;
		}
		return true;
	}

	// Check if the player is moving on the horizontal plane.
	public bool IsHorizontalMoving()
	{
		return h != 0;
	}

	// Check if the player is moving.
	public bool IsMoving()
	{
		return (h != 0)|| (v != 0);
	}

	// Get the last player direction of facing.
	public Vector3 GetLastDirection()
	{
		return lastDirection;
	}

	// Set the last player direction of facing.
	public void SetLastDirection(Vector3 direction)
	{
		lastDirection = direction;
	}

	// Put the player on a standing up position based on last direction faced.
	public void Repositioning()
	{
		if(lastDirection != Vector3.zero)
		{
			lastDirection.y = 0;
			Quaternion targetRotation = Quaternion.LookRotation (lastDirection);
			Quaternion newRotation = Quaternion.Slerp(rBody.rotation, targetRotation, GlobalSettings.turnSmoothing);
			rBody.MoveRotation (newRotation);
		}
	}

	// Function to tell whether or not the player is on ground.
	// Kept it in this class to facilitate its use in other classes.
	public bool IsGrounded()
	{
		Ray ray = new(transform.position + Vector3.up * 2 * colExtents.x, Vector3.down);
		return Physics.SphereCast(ray, colExtents.x, colExtents.x + 0.2f);
	}
}