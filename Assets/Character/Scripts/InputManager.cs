using UnityEngine;

// Class to manage every input.
public static class InputManager
{
    // Defining button and axis names
    public const string HorizontalAxis = "Horizontal";           // Default move button (q/d or left/right arrow referenced in ProjectSettings/InputManager.asset).
    public const string VerticalAxis = "Vertical";               // Default move button (z/s or up/down arrow referenced in ProjectSettings/InputManager.asset).
    public const string AimButton = "Aim";                       // Default aim button (right mouse button referenced in ProjectSettings/InputManager.asset).
    public const string SprintButton = "Sprint";                 // Default sprint button (left shift referenced in ProjectSettings/InputManager.asset).
    public const string JumpButton = "Jump";                     // Default jump button (space bar referenced in ProjectSettings/InputManager.asset).
    public const string ShoulderButton = "Aim Shoulder";         // Default switch shoulders button (middle mouse button referenced in ProjectSettings/InputManager.asset).
    public const string MouseXAxis = "Mouse X";                  // Default horizontal axis mouse input name (referenced in ProjectSettings/InputManager.asset).
    public const string MouseYAxis = "Mouse Y";                  // Default vertical axis mouse input name (referenced in ProjectSettings/InputManager.asset).
    public const string AnalogXAxis = "Analog X";                // Default horizontal axis joystick input name (referenced in ProjectSettings/InputManager.asset).
    public const string AnalogYAxis = "Analog Y";                // Default vertical axis joystick input name (referenced in ProjectSettings/InputManager.asset).
    public const string MouseScrollWheel = "Mouse ScrollWheel";  // Default mouse scrollwheel name (used to adapt move speed referenced in ProjectSettings/InputManager.asset).
    public const string AttackButton = "Fire1";                  // Default attack button (left click referenced in ProjectSettings/InputManager.asset).

    // Boolean functions to know if a button is pressed or not.
    // Return true if the player pressed the JumpButton.
    public static bool IsJumping()
    {
        return Input.GetButtonDown(JumpButton);
    }

    // Return true if the SprintButton is held down.
    public static bool IsSprinting()
    {
        return Input.GetButton(SprintButton);
    }

    // Return true if the player pressed the AimButton.
    public static bool IsAiming()
    {
        return Input.GetAxisRaw(AimButton) != 0;
    }

    // Return true if the player pressed the ShoulderButton.
    public static bool SwitchShoulder()
    {
        return Input.GetButtonDown(ShoulderButton);
    }

    // Return true if the player pressed the AttackButton.
    public static bool IsAttacking()
    {
        return Input.GetButtonDown(AttackButton);
    }


    // Functions to store different values.
    public static float GetHorizontalAxis()
    {
        return Input.GetAxis(HorizontalAxis);
    }

    public static float GetVerticalAxis()
    {
        return Input.GetAxis(VerticalAxis);
    }

    public static float GetScrollWheelAxis(){
        return Input.GetAxis(MouseScrollWheel);
    }

    public static float GetMouseX()
    {
        return Input.GetAxis(MouseXAxis);
    }

    public static float GetMouseY()
    {
        return Input.GetAxis(MouseYAxis);
    }

    public static float GetAnalogX()
    {
        return Input.GetAxis(AnalogXAxis);
    }

    public static float GetAnalogY()
    {
        return Input.GetAxis(AnalogYAxis);
    }
}
