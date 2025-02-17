using UnityEngine;

// This class corresponds to the 3rd person camera features.
public class ThirdPersonOrbitCamBasic : MonoBehaviour 
{
	public Transform player;                                           // Player's reference.
	private Transform cam;                                             // This transform.
	private Vector3 smoothPivotOffset;                                 // Camera current pivot offset on interpolation.
	private Vector3 smoothCamOffset;                                   // Camera current offset on interpolation.
	private Vector3 targetPivotOffset;                                 // Camera pivot offset target to iterpolate.
	private Vector3 targetCamOffset;                                   // Camera offset target to interpolate.
	private float defaultFOV;                                          // Default camera Field of View.
	private float targetFOV;                                           // Target camera Field of View.
	private float targetMaxVerticalAngle;                              // Custom camera max vertical clamp angle.
	private bool isCustomOffset;                                       // Boolean to determine whether or not a custom camera offset is being used.

	// Get the camera horizontal angle.
	public float GetH { get { return GlobalSettings.angleH; } }

	void Awake()
	{
		// Reference to the camera transform.
		cam = transform;

		// Set camera default position.
		cam.SetPositionAndRotation(player.position + Quaternion.identity * GlobalSettings.pivotOffset + Quaternion.identity * GlobalSettings.camOffset, Quaternion.identity);

        // Set up references and default values.
        smoothPivotOffset = GlobalSettings.pivotOffset;
		smoothCamOffset = GlobalSettings.camOffset;
		defaultFOV = cam.GetComponent<Camera>().fieldOfView;
		GlobalSettings.angleH = player.eulerAngles.y;

		ResetTargetOffsets ();
		ResetFOV ();
		ResetMaxVerticalAngle();

		// Check for no vertical offset.
		if (GlobalSettings.camOffset.y > 0)
			Debug.LogWarning("Vertical offset is ignored during collision");
	}

	void Update()
	{
		// Get movement to orbit the camera.
		// Mouse:
		GlobalSettings.angleH += Mathf.Clamp(InputManager.GetMouseX(), -1, 1) * GlobalSettings.horizontalAimingSpeed;
		GlobalSettings.angleV += Mathf.Clamp(InputManager.GetMouseY(), -1, 1) * GlobalSettings.verticalAimingSpeed;
		// Joystick:
		GlobalSettings.angleH += Mathf.Clamp(InputManager.GetAnalogX(), -1, 1) * 60 * GlobalSettings.horizontalAimingSpeed * Time.deltaTime;
		GlobalSettings.angleV += Mathf.Clamp(InputManager.GetAnalogY(), -1, 1) * 60 * GlobalSettings.verticalAimingSpeed * Time.deltaTime;

		// Set vertical movement limit.
		GlobalSettings.angleV = Mathf.Clamp(GlobalSettings.angleV, GlobalSettings.minVerticalAngle, targetMaxVerticalAngle);

		// Set camera orientation.
		Quaternion camYRotation = Quaternion.Euler(0, GlobalSettings.angleH, 0);
		Quaternion aimRotation = Quaternion.Euler(-GlobalSettings.angleV, GlobalSettings.angleH, 0);
		cam.rotation = aimRotation;

		// Set FOV.
		cam.GetComponent<Camera>().fieldOfView = Mathf.Lerp (cam.GetComponent<Camera>().fieldOfView, targetFOV,  Time.deltaTime);

		// Test for collision with the environment based on current camera position.
		Vector3 baseTempPosition = player.position + camYRotation * targetPivotOffset;
		Vector3 noCollisionOffset = targetCamOffset;
		while (noCollisionOffset.magnitude >= 0.2f)
		{
			if (CollisionManager.DoubleViewingPosCheck(baseTempPosition + aimRotation * noCollisionOffset, player))
				break;
			noCollisionOffset -= noCollisionOffset.normalized * 0.2f;
		}
		if (noCollisionOffset.magnitude < 0.2f)
			noCollisionOffset = Vector3.zero;

		// No intermediate position for custom offsets, go to 1st person.
		bool customOffsetCollision = isCustomOffset && noCollisionOffset.sqrMagnitude < targetCamOffset.sqrMagnitude;

		// Repostition the camera.
		smoothPivotOffset = Vector3.Lerp(smoothPivotOffset, customOffsetCollision ? GlobalSettings.pivotOffset : targetPivotOffset, GlobalSettings.smooth * Time.deltaTime);
		smoothCamOffset = Vector3.Lerp(smoothCamOffset, customOffsetCollision ? Vector3.zero : noCollisionOffset, GlobalSettings.smooth * Time.deltaTime);

		cam.position =  player.position + camYRotation * smoothPivotOffset + aimRotation * smoothCamOffset;
	}

	// Set camera offsets to custom values.
	public void SetTargetOffsets(Vector3 newPivotOffset, Vector3 newCamOffset)
	{
		targetPivotOffset = newPivotOffset;
		targetCamOffset = newCamOffset;
		isCustomOffset = true;
	}

	// Reset camera offsets to default values.
	public void ResetTargetOffsets()
	{
		targetPivotOffset = GlobalSettings.pivotOffset;
		targetCamOffset = GlobalSettings.camOffset;
		isCustomOffset = false;
	}

	// Reset the camera vertical offset.
	public void ResetYCamOffset()
	{
		targetCamOffset.y = GlobalSettings.camOffset.y;
	}

	// Set camera vertical offset.
	public void SetYCamOffset(float y)
	{
		targetCamOffset.y = y;
	}

	// Set camera horizontal offset.
	public void SetXCamOffset(float x)
	{
		targetCamOffset.x = x;
	}

	// Set custom Field of View.
	public void SetFOV(float customFOV)
	{
		targetFOV = customFOV;
	}

	// Reset Field of View to default value.
	public void ResetFOV()
	{
		targetFOV = defaultFOV;
	}

	// Set max vertical camera rotation angle.
	public void SetMaxVerticalAngle(float angle)
	{
		targetMaxVerticalAngle = angle;
	}

	// Reset max vertical camera rotation angle to default value.
	public void ResetMaxVerticalAngle()
	{
		targetMaxVerticalAngle = GlobalSettings.maxVerticalAngle;
	}

	// Get camera magnitude.
	public float GetCurrentPivotMagnitude(Vector3 finalPivotOffset)
	{
		return Mathf.Abs ((finalPivotOffset - smoothPivotOffset).magnitude);
	}
}
