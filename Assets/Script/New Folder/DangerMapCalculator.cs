using UnityEngine;
using System.Collections.Generic;

public class DangerMapCalculator
{
    private readonly Compass compass;
    private float[] dangerMap;

    public DangerMapCalculator()
    {
        compass = Compass.Instance;
        dangerMap = new float[compass.GetDirectionCount()]; // Initialize once with the correct size
    }

    public float[] CalculateDangerMap(List<Obstacle_Struct> obstacles, Vector2 position)
    {
        // Ensure the danger map is correctly sized
        if (dangerMap.Length != compass.GetDirectionCount())
        {
            dangerMap = new float[compass.GetDirectionCount()];
        }

        // Clear previous values in the danger map
        System.Array.Clear(dangerMap, 0, dangerMap.Length);

        // Calculate influence for each obstacle
        foreach (Obstacle_Struct obstacle in obstacles)
        {
            Vector2 directionToObstacle = ((Vector2)obstacle.position - position).normalized;

            // Calculate influence based on alignment with each compass direction
            for (int i = 0; i < compass.GetDirectionCount(); i++)
            {
                float dotProduct = Vector2.Dot(compass.GetDirection(i), directionToObstacle);
                dangerMap[i] += Mathf.Max(0, dotProduct); // Add influence only if alignment is positive
            }
        }

        // Optional: Normalize the map if you want values between 0 and 1
        float maxInfluence = Mathf.Max(dangerMap); // Find the highest influence value
        if (maxInfluence > 0)
        {
            for (int i = 0; i < dangerMap.Length; i++)
            {
                dangerMap[i] /= maxInfluence; // Scale all values to be between 0 and 1
            }
        }

        return dangerMap;
    }
}
