using UnityEngine;
using System.Collections;

// AimBehaviour inherits from GenericBehaviour. This class corresponds to aim and strafe behaviour.
public class AimBehaviour : GenericBehaviour
{
	public Texture2D crosshair;                            // Crosshair texture.
	private bool aim;                                      // Boolean to determine whether or not the player is aiming.
    private AnimationManager animationManager;             // Management of the different animations.

    // Start is always called after any Awake functions.
    void Start ()
	{
		// Set up the references.
		animationManager = new AnimationManager(behaviourManager.GetAnim);
	}

	// Update is used to set features regardless the active behaviour.
	void Update ()
	{
		// Activate/deactivate aim by input.
		if (InputManager.IsAiming() && !aim)
		{
			StartCoroutine(ToggleAimOn());
		}
		else if (aim && !InputManager.IsAiming())
		{
			StartCoroutine(ToggleAimOff());
		}

		// No sprinting while aiming.
		canSprint = !aim;

		// Toggle camera aim position left or right, switching shoulders.
		if (aim && InputManager.SwitchShoulder())
		{
			GlobalSettings.aimCamOffset.x *= -1;
			GlobalSettings.aimPivotOffset.x *= -1;
		}

		// Set aim boolean on the Animator Controller.
		animationManager.SetAim(aim);
	}

	// Co-rountine to start aiming mode with delay.
	private IEnumerator ToggleAimOn()
	{
		yield return new WaitForSeconds(0.05f);
        // Aiming is not possible.
        if (behaviourManager.GetTempLockStatus(behaviourCode) || behaviourManager.IsOverriding(this))
            yield return false;
        // Start aiming.
        else
        {
            aim = true;
            int signal = 1;
            GlobalSettings.aimCamOffset.x = Mathf.Abs(GlobalSettings.aimCamOffset.x) * signal;
            GlobalSettings.aimPivotOffset.x = Mathf.Abs(GlobalSettings.aimPivotOffset.x) * signal;
            yield return new WaitForSeconds(0.1f);
            animationManager.SetSpeed(0);
            // This state overrides the active one.
            behaviourManager.OverrideWithBehaviour(this);
        }
    }

	// Co-rountine to end aiming mode with delay.
	private IEnumerator ToggleAimOff()
	{
		aim = false;
		yield return new WaitForSeconds(0.3f);
		behaviourManager.GetCamScript.ResetTargetOffsets();
		behaviourManager.GetCamScript.ResetMaxVerticalAngle();
		yield return new WaitForSeconds(0.05f);
		behaviourManager.RevokeOverridingBehaviour(this);
	}

	// LocalFixedUpdate overrides the virtual function of the base class.
	public override void LocalFixedUpdate()
	{
		// Set camera position and orientation to the aim mode parameters.
		if(aim)
			behaviourManager.GetCamScript.SetTargetOffsets (GlobalSettings.aimPivotOffset, GlobalSettings.aimCamOffset);
	}

	// LocalLateUpdate: manager is called here to set player rotation after camera rotates, avoiding flickering.
	public override void LocalLateUpdate()
	{
		Aim();
	}

	// Handle aim parameters when aiming is active.
	void Aim()
	{
		// Deal with the player orientation when aiming.
		Rotating();
	}

	// Rotate the player to match correct orientation, according to camera.
	void Rotating()
	{
		Vector3 forward = behaviourManager.playerCamera.TransformDirection(Vector3.forward);
		// Player is moving on ground, Y component of camera facing is not relevant.
		forward.y = 0.0f;
		forward = forward.normalized;

		// Always rotates the player according to the camera horizontal rotation in aim mode.
		Quaternion targetRotation =  Quaternion.Euler(0, behaviourManager.GetCamScript.GetH, 0);

		float minSpeed = Quaternion.Angle(transform.rotation, targetRotation) * GlobalSettings.aimTurnSmoothing;

		// Rotate entire player to face camera.
		behaviourManager.SetLastDirection(forward);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, minSpeed * Time.deltaTime);

	}

 	// Draw the crosshair when aiming.
	void OnGUI () 
	{
		if (crosshair)
		{
			float mag = behaviourManager.GetCamScript.GetCurrentPivotMagnitude(GlobalSettings.aimPivotOffset);
			if (mag < 0.05f)
				GUI.DrawTexture(new Rect(Screen.width / 2 - (crosshair.width * 0.5f),
										 Screen.height / 2 - (crosshair.height * 0.5f),
										 crosshair.width, crosshair.height), crosshair);
		}
	}
}
