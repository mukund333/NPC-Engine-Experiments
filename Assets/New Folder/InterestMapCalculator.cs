using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class InterestMapCalculator
{
    private readonly Compass compass;
    private float[] interestMap;

    public InterestMapCalculator()
    {
        compass = Compass.Instance;
        interestMap = new float[compass.GetDirectionCount()]; // Initialize once with the correct size
    }

    public float[] CalculateInterestMap(Vector3 target, Vector2 position)
    {
        Vector2 directionToTarget = ((Vector2)target - position).normalized;

        // Check if interestMap needs resizing in case the direction count changes
        if (interestMap.Length != compass.GetDirectionCount())
        {
            interestMap = new float[compass.GetDirectionCount()];
        }

        // Populate interestMap based on alignment with directionToTarget
        for (int i = 0; i < compass.GetDirectionCount(); i++)
        {
            float dotProduct = Vector2.Dot(compass.GetDirection(i), directionToTarget);
            interestMap[i] = Mathf.Max(0, dotProduct); // Interest is zero if the dot product is negative
        }

        return interestMap;
    }
}
