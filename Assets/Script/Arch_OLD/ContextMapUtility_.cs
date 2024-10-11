using UnityEngine;
using System.Linq;

public static class ContextMapUtility_
{
    // Calculates the context map for a given target or obstacle
    public static float[] CalculateContextMap(Vector2 position, Vector2 targetPosition, int numSlots, int falloffRange, bool isDanger)
    {
        float[] contextMap = new float[numSlots];
        float anglePerSlot = 360f / numSlots;

        // Calculate direction to target or obstacle
        Vector2 direction = (targetPosition - position).normalized;
        float angle = Vector2.SignedAngle(Vector2.up, direction); // Get angle relative to the forward direction
        if (angle < 0) angle += 360f; // Convert angle to a range of 0 to 360 degrees

        // Convert angle to a slot index in the context map
        int slotIndex = Mathf.RoundToInt(angle / anglePerSlot) % numSlots;

        // Calculate base value based on distance (higher value closer to target/obstacle)
        float distance = Vector2.Distance(position, targetPosition);
        float baseValue = 1 / (distance + 1); // Normalize to prevent division by zero

        // Adjust base value for interest (seek) or danger (avoid)
        if (isDanger)
        {
            baseValue *= -1; // Danger values should be negative to reduce desirability
        }

        // Apply base value to the slot and nearby slots with falloff
        for (int i = -falloffRange; i <= falloffRange; i++)
        {
            int index = (slotIndex + i + numSlots) % numSlots;
            float falloff = 1f / (Mathf.Abs(i) + 1);
            contextMap[index] += baseValue * falloff;
        }

        // Normalize the context map values to ensure they are within a standard range
        float maxValue = contextMap.Max();
        if (maxValue > 1)
        {
            for (int i = 0; i < numSlots; i++)
            {
                contextMap[i] /= maxValue;
            }
        }

        return contextMap;
    }

    // Smooths the context map using neighboring values to reduce sharp changes
    public static float[] SmoothContextMap(float[] contextMap, int smoothingRadius)
    {
        int numSlots = contextMap.Length;
        float[] smoothedMap = new float[numSlots];

        for (int i = 0; i < numSlots; i++)
        {
            float sum = 0f;
            int count = 0;

            // Sum the values within the smoothing radius
            for (int offset = -smoothingRadius; offset <= smoothingRadius; offset++)
            {
                int neighborIndex = (i + offset + numSlots) % numSlots;
                sum += contextMap[neighborIndex];
                count++;
            }

            // Calculate the smoothed value as the average
            smoothedMap[i] = sum / count;
        }

        return smoothedMap;
    }
}
