using UnityEngine;
public class BoxCastDrawer2D
{
    public static void DrawBoxCast(Vector2 origin, Vector2 direction, Vector2 size, float distance, bool hasHit)
    {
        //if (hasHit)
        //    Debug.Log("hit");
        Color boxColor = hasHit ? Color.red : Color.green;
        Vector2 endPoint = origin + direction * distance;
        // Draw start box
        DrawBox(origin, size, boxColor);
        // Draw end box
        DrawBox(endPoint, size, boxColor);
        // Draw connecting lines
        DrawConnectingLines(origin, endPoint, size, boxColor);
    }

    private static void DrawBox(Vector2 center, Vector2 size, Color color)
    {
        // Calculate corners
        Vector2 bottomLeft = center + new Vector2(-size.x / 2, -size.y / 2);
        Vector2 bottomRight = center + new Vector2(size.x / 2, -size.y / 2);
        Vector2 topLeft = center + new Vector2(-size.x / 2, size.y / 2);
        Vector2 topRight = center + new Vector2(size.x / 2, size.y / 2);
        // Draw box outline
        Debug.DrawLine(bottomLeft, bottomRight, color);
        Debug.DrawLine(bottomRight, topRight, color);
        Debug.DrawLine(topRight, topLeft, color);
        Debug.DrawLine(topLeft, bottomLeft, color);
    }

    private static void DrawConnectingLines(Vector2 start, Vector2 end, Vector2 size, Color color)
    {
        // Draw lines connecting the corners of start and end boxes
        Vector2 bottomLeft = new Vector2(-size.x / 2, -size.y / 2);
        Vector2 bottomRight = new Vector2(size.x / 2, -size.y / 2);
        Vector2 topLeft = new Vector2(-size.x / 2, size.y / 2);
        Vector2 topRight = new Vector2(size.x / 2, size.y / 2);
        Debug.DrawLine(start + bottomLeft, end + bottomLeft, color);
        Debug.DrawLine(start + bottomRight, end + bottomRight, color);
        Debug.DrawLine(start + topLeft, end + topLeft, color);
        Debug.DrawLine(start + topRight, end + topRight, color);
    }
}