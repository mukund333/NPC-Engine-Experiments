using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
public static class ContextResolver_
{
    // Resolves the final direction based on combined interest and danger maps
    public static Vector2 ResolveContexts(float[] interestMap, float[] dangerMap, int numSlots)
    {
        float[] combinedMap = new float[numSlots];

        // Combine the interest and danger maps
        for (int i = 0; i < numSlots; i++)
        {
            combinedMap[i] = interestMap[i] + dangerMap[i]; // Combine by summing interest and danger values
        }

        // Find the slot with the maximum value in the combined map
        int bestSlot = System.Array.IndexOf(combinedMap, combinedMap.Max());
        float anglePerSlot = 360f / numSlots;
        float bestAngle = bestSlot * anglePerSlot;

        // Convert the best slot index back into a direction vector
        return Quaternion.Euler(0, 0, bestAngle) * Vector2.up; // Rotate "up" by the best angle
    }
}
