using UnityEngine;

public static class GlobalSettings
{
    // Aiming settings
    public const float aimTurnSmoothing = 0.15f;                         // Speed of turn response when aiming to match camera facing.
    public static Vector3 aimPivotOffset = new(0.5f, 1.2f,  0f);         // Offset to repoint the camera when aiming.
	public static Vector3 aimCamOffset   = new(0f, 0.4f, -0.7f);         // Offset to relocate the camera when aiming.
    
    // Moving settings
    public const float walkSpeed = 0.15f;                                // Default walk speed.
	public const float runSpeed = 1.0f;                                  // Default run speed.
	public const float sprintSpeed = 2.0f;                               // Default sprint speed.
	public const float speedDampTime = 0.1f;                             // Default damp time to change the animations based on current speed.
    public const float turnSmoothing = 0.06f;                            // Speed of turn when moving to match camera facing.
    
    // Jumping settings
    public const float jumpHeight = 1.5f;                                // Default jump height.
	public const float jumpHorizontalForce = 10f;                        // Default horizontal inertial force when jumping.

    // Camera settings
    public static Vector3 pivotOffset = new(0.0f, 1.7f,  0.0f);          // Offset to repoint the camera.
	public static Vector3 camOffset = new(0.0f, 0.0f, -3.0f);            // Offset to relocate the camera related to the player position.
	public const float smooth = 10f;                                     // Speed of camera responsiveness.
	public const float horizontalAimingSpeed = 6f;                       // Horizontal turn speed.
	public const float verticalAimingSpeed = 6f;                         // Vertical turn speed.
	public const float maxVerticalAngle = 30f;                           // Camera max clamp angle. 
	public const float minVerticalAngle = -60f;                          // Camera min clamp angle.
	public static float angleH = 0;                                      // Float to store camera horizontal angle related to mouse movement.
	public static float angleV = 0;                                      // Float to store camera vertical angle related to mouse movement.
}