using UnityEngine;
using System.Collections.Generic;



public class DangerMapCalculator
{
    private Vector2[] directions;
    private float maxDistance =5f; // Maximum detection distance

    public DangerMapCalculator(int directionCount = 16)
    {
        InitializeDirections(directionCount);
    }

    private void InitializeDirections(int count)
    {
        directions = new Vector2[count];
        float angleStep = 360f / count;
        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep;
            float rad = angle * Mathf.Deg2Rad;
            directions[i] = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
        }
    }

    private int GetDirectionSpreadForDistance(float distance)
    {
        float normalizedDistance = distance / maxDistance;

        // Strict distance-based spread rules:
        if (normalizedDistance >= 0.8f) // Far away (80-100% of max distance)
            return 3;  // Influence 3 directions
        else if (normalizedDistance >= 0.5f) // Medium distance (50-80% of max distance)
            return 5;  // Influence 5 directions
        else // Close distance (0-50% of max distance)
            return 7;  // Influence 7 directions
    }

    public float[] CalculateDangerMap(List<Obstacle_Struct> obstacles, Vector2 position)
    {
        float[] dangerMap = new float[directions.Length];
        float[] directionalInfluence = new float[directions.Length];
        int totalInfluences = 0;

        foreach (Obstacle_Struct obstacle in obstacles)
        {
            float distance = Vector2.Distance(obstacle.position, position);
            if (distance > 0f && distance <= maxDistance)
            {
                Vector2 directionToTarget = ((Vector2)obstacle.position - position).normalized;
                int mainDirectionIndex = GetClosestDirectionIndex(directionToTarget);

                // Get exact number of directions to influence based on distance
                int spreadCount = GetDirectionSpreadForDistance(distance);

                // Apply influence to directions
                ApplyDirectionalInfluence(directionalInfluence, mainDirectionIndex, spreadCount);
                totalInfluences++;

                //Debug.Log($"Target at distance {distance:F2} influences {spreadCount} directions. Main direction: {mainDirectionIndex}");
            }
        }

        if (totalInfluences == 0) return dangerMap;

        // Normalize the interest values
        for (int i = 0; i < directions.Length; i++)
        {
            dangerMap[i] = directionalInfluence[i] / totalInfluences;
            //Debug.Log($"<color=red>Direction {i}: {directions[i]} - Danger Value: {dangerMap[i]}</color>");

        }

        return dangerMap;
    }

    private int GetClosestDirectionIndex(Vector2 targetDirection)
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

    private void ApplyDirectionalInfluence(float[] directionalInfluence, int centerIndex, int spreadCount)
    {
        int spreadRadius = spreadCount / 2;

        for (int offset = -spreadRadius; offset <= spreadRadius; offset++)
        {
            // Calculate the actual index with wraparound
            int index = (centerIndex + offset + directions.Length) % directions.Length;

            // Calculate falloff based on distance from center direction
            float falloff = 1f - (Mathf.Abs(offset) / (float)(spreadRadius + 1));

            // Apply influence with falloff
            directionalInfluence[index] += falloff;
        }
    }

    public Vector2[] GetDirections() => directions;


   
}