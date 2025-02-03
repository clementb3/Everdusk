using UnityEngine;

public static class CollisionManager
{
    // Camera collision : 

    // Check for collision from camera to player.
    public static bool ViewingPosCheck(Vector3 checkPos, Transform player)
    {
        Vector3 target = player.position + GlobalSettings.pivotOffset;
        Vector3 direction = target - checkPos;

        // If a raycast from the check position to the player hits something
        if (Physics.SphereCast(checkPos, 0.2f, direction, out RaycastHit hit, direction.magnitude))
        {
            // Check if it's the player 
            return hit.transform == player || hit.transform.GetComponent<Collider>().isTrigger;
        }
        return true;
    }
    public static bool ReverseViewingPosCheck(Vector3 checkPos, Transform player)
    {
        Vector3 origin = player.position + GlobalSettings.pivotOffset;
        Vector3 direction = checkPos - origin;

        if (Physics.SphereCast(origin, 0.2f, direction, out RaycastHit hit, direction.magnitude))
        {
            return hit.transform == player || hit.transform.GetComponent<Collider>().isTrigger;
        }
        return true;
    }

    // Double check for collisions: concave objects doesn't detect hit from outside, so cast in both directions.
    public static bool DoubleViewingPosCheck(Vector3 checkPos, Transform player)
    {
        return ViewingPosCheck(checkPos, player) && ReverseViewingPosCheck(checkPos, player);
    }
}
