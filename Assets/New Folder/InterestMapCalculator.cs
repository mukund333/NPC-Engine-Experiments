using UnityEngine;
using System.Collections.Generic;



public class InterestMapCalculator
{
    private Vector2[] directions;



    public InterestMapCalculator(int directionCount = 16)
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


    public float[] CalculateInterestMap(List<Target_Struct> targets, Vector2 position)
    {
        float[] interestMap = new float[directions.Length];
        float[] weightedSum = new float[directions.Length];
        float totalWeight = 0f;

        // Sum up weights for denominator
        foreach (Target_Struct target in targets)
        {
            totalWeight += target.weight;
        }

        if (totalWeight <= 0f) return interestMap;

        foreach (Target_Struct target in targets)
        {
            Vector2 directionToTarget = ((Vector2)target.position - position).normalized;

            for (int i = 0; i < directions.Length; i++)
            {
                float dotProduct = Vector2.Dot(directions[i], directionToTarget);
                float interest = Mathf.Max(0, dotProduct);
                weightedSum[i] += interest * target.weight;
            }
        }

        for (int i = 0; i < directions.Length; i++)
        {
            interestMap[i] = weightedSum[i] / totalWeight;
        }

        return interestMap;
    }

    public Vector2[] GetDirections() => directions;
}