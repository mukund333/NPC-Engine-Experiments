using UnityEngine;
using System.Collections.Generic;

public class AvoidBehavior_ : IBehavior_
{
    private List<Vector2> obstaclePositions = new List<Vector2>();
    public float Weight { get; private set; } = 1.0f; // Default weight

    // Add an obstacle position to avoid
    public void AddObstacle(Vector2 obstacle)
    {
        obstaclePositions.Add(obstacle);
    }

    // Calculate the influence map for avoiding behavior
    public float[] CalculateInfluenceMap(Vector2 entityPosition, int numSlots, int falloffRange)
    {
        float[] combinedMap = new float[numSlots];

        // Calculate influence for each obstacle and combine them
        foreach (var obstacle in obstaclePositions)
        {
            float[] obstacleMap = ContextMapUtility_.CalculateContextMap(entityPosition, obstacle, numSlots, falloffRange, true);
            CombineMaps(combinedMap, obstacleMap);
        }

        return combinedMap;
    }

    // Helper function to combine multiple influence maps
    private void CombineMaps(float[] baseMap, float[] additionalMap)
    {
        for (int i = 0; i < baseMap.Length; i++)
        {
            baseMap[i] += additionalMap[i];
        }
    }
}
