using UnityEngine;

public static class DirectionUtility
{
    public static Vector2 GetFOVDirection(FOVDirection direction)
    {
        // Existing method remains unchanged
        switch (direction)
        {
            case FOVDirection.Forward: return Vector2.up;
            case FOVDirection.Backward: return Vector2.down;
            case FOVDirection.Left: return Vector2.left;
            case FOVDirection.Right: return Vector2.right;
            case FOVDirection.TopLeft: return new Vector2(-1, 1).normalized;
            case FOVDirection.TopRight: return new Vector2(1, 1).normalized;
            case FOVDirection.BottomLeft: return new Vector2(-1, -1).normalized;
            case FOVDirection.BottomRight: return new Vector2(1, -1).normalized;
            default: return Vector2.zero;
        }
    }

    // New method to get direction based on transform
    public static Vector2 GetFOVDirectionFromTransform(Transform transform, FOVDirection baseDirection)
    {
        Vector2 baseVector = GetFOVDirection(baseDirection);
        return transform.TransformDirection(baseVector);
    }
}