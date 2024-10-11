using UnityEngine;
using System.Collections.Generic;

public class SeekBehavior_ : IBehavior_
{
    private List<Vector2> targetPositions = new List<Vector2>();
    public float Weight { get; private set; } = 1.0f; // Default weight

    // Add a target position to seek
    public void AddTarget(Vector2 target)
    {
        targetPositions.Add(target);
    }

    // Calculate the influence map for seeking behavior
    public float[] CalculateInfluenceMap(Vector2 entityPosition, int numSlots, int falloffRange)
    {
        float[] combinedMap = new float[numSlots];

        // Calculate influence for each target and combine them
        foreach (var target in targetPositions)
        {
            float[] targetMap = ContextMapUtility_.CalculateContextMap(entityPosition, target, numSlots, falloffRange, false);
            CombineMaps(combinedMap, targetMap);
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
