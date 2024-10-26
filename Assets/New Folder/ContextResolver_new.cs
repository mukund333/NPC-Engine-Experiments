using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ContextResolver_new 
{
    static float[] combinedMap = new float[NumberOfSlots.slotsLength];

    public static Vector2 CalculateSteeringDirection(float[] interestMap, float[] dangerMap, Vector2[] directions)
    {
        for (int i = 0; i < combinedMap.Length; i++)
        {
            combinedMap[i] = interestMap[i] - dangerMap[i];  // Combine interest and danger maps
        }

        // Find the direction with the highest score
        int bestDirectionIndex = 0;
        float highestScore = float.MinValue;
        for (int i = 0; i < combinedMap.Length; i++)
        {
            if (combinedMap[i] > highestScore)
            {
                highestScore = combinedMap[i];
                bestDirectionIndex = i;
            }
        }

        return directions[bestDirectionIndex];  // Return the best direction vector
    }

}
