using UnityEngine;

public class Compass
{
    private Vector2[] directions;
    private int directionCount;

    // Singleton pattern implementation
    private static Compass instance;
    public static Compass Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Compass();
            }
            return instance;
        }
    }

    // Private constructor to prevent instantiation
    private Compass(int numberOfDirections = 16)
    {
        InitializeDirections(numberOfDirections);
    }

    private void InitializeDirections(int count)
    {
        directionCount = count;
        directions = new Vector2[count];
        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep;
            float rad = angle * Mathf.Deg2Rad;
            directions[i] = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
        }
    }

    // Getter methods
    public Vector2[] GetAllDirections() => directions;

    public Vector2 GetDirection(int index)
    {
        return directions[Mathf.Clamp(index, 0, directions.Length - 1)];
    }

    public int GetDirectionCount() => directionCount;

    // Find closest direction index to a target direction
    public int GetClosestDirectionIndex(Vector2 targetDirection)
    {
        float maxDot = -1f;
        int closestIndex = 0;

        for (int i = 0; i < directions.Length; i++)
        {
            float dot = Vector2.Dot(targetDirection, directions[i]);
            if (dot > maxDot)
            {
                maxDot = dot;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    // Get neighboring direction indices for a spread around a center index
    public int[] GetNeighborIndices(int centerIndex, int spread)
    {
        int[] indices = new int[spread];
        int halfSpread = spread / 2;

        for (int i = -halfSpread; i <= halfSpread; i++)
        {
            int index = i + halfSpread;
            indices[index] = WrapDirectionIndex(centerIndex + i);
        }

        return indices;
    }

    private int WrapDirectionIndex(int index)
    {
        return ((index % directionCount) + directionCount) % directionCount;
    }

    // Calculate a falloff value based on distance from the center index
    public float GetFalloffValue(int centerIndex, int currentIndex, int spreadRadius)
    {
        int distance = Mathf.Abs(WrapDirectionIndex(currentIndex - centerIndex));
        distance = Mathf.Min(distance, directionCount - distance);
        return 1f - (distance / (float)(spreadRadius + 1));
    }
}
