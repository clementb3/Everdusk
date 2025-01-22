using UnityEngine;

// This is the base class for all player behaviours
public abstract class GenericBehaviour : MonoBehaviour
{
	protected int speedFloat;                      // Speed parameter on the Animator.
	protected BasicBehaviour behaviourManager;     // Reference to the basic behaviour manager.
	protected int behaviourCode;                   // The code that identifies a behaviour.
	protected bool canSprint;                      // Boolean to store if the behaviour allows the player to sprint.

	void Awake()
	{
		// Set up the references.
		behaviourManager = GetComponent<BasicBehaviour> ();
		speedFloat = Animator.StringToHash("Speed");
		canSprint = true;

		// Set the behaviour code based on the inheriting class.
		behaviourCode = this.GetType().GetHashCode();
	}

	// The active behaviour will control the player actions with these functions:
	// The local equivalent for MonoBehaviour's FixedUpdate function.
	public virtual void LocalFixedUpdate() { }

	// The local equivalent for MonoBehaviour's LateUpdate function.
	public virtual void LocalLateUpdate() { }

	// This function is called when another behaviour overrides the current one.
	public virtual void OnOverride() { }

	// Get the behaviour code.
	public int GetBehaviourCode()
	{
		return behaviourCode;
	}

	// Check if the behaviour allows sprinting.
	public bool AllowSprint()
	{
		return canSprint;
	}
}