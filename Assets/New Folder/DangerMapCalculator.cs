using UnityEngine;
using System.Collections.Generic;

public class DangerMapCalculator
{
    private readonly Compass compass;
    private float maxDistance = 5f;

    public DangerMapCalculator()
    {
        compass = Compass.Instance;
    }

    private int GetDirectionSpreadForDistance(float distance)
    {
        float normalizedDistance = distance / maxDistance;
        if (normalizedDistance >= 0.8f)
            return 3;
        else if (normalizedDistance >= 0.5f)
            return 5;
        return 7;
    }

    public float[] CalculateDangerMap(List<Obstacle_Struct> obstacles, Vector2 position)
    {
        int directionCount = compass.GetDirectionCount();
        float[] dangerMap = new float[directionCount];
        float[] directionalInfluence = new float[directionCount];
        int totalInfluences = 0;

        foreach (Obstacle_Struct obstacle in obstacles)
        {
            float distance = Vector2.Distance(obstacle.position, position);
            if (distance > 0f && distance <= maxDistance)
            {
                Vector2 directionToTarget = ((Vector2)obstacle.position - position).normalized;
                int mainDirectionIndex = compass.GetClosestDirectionIndex(directionToTarget);
                int spreadCount = GetDirectionSpreadForDistance(distance);

                ApplyDirectionalInfluence(directionalInfluence, mainDirectionIndex, spreadCount);
                totalInfluences++;
            }
        }

        if (totalInfluences > 0)
        {
            for (int i = 0; i < directionCount; i++)
            {
                dangerMap[i] = directionalInfluence[i] / totalInfluences;
            }
        }

        return dangerMap;
    }

    private void ApplyDirectionalInfluence(float[] directionalInfluence, int centerIndex, int spreadCount)
    {
        int spreadRadius = spreadCount / 2;
        int[] affectedIndices = compass.GetNeighborIndices(centerIndex, spreadCount);

        foreach (int index in affectedIndices)
        {
            float falloff = compass.GetFalloffValue(centerIndex, index, spreadRadius);
            directionalInfluence[index] += falloff;
        }
    }
}
