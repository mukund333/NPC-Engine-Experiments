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


    public float[] CalculateInterestMap(Vector3 target, Vector2 position)
    {

        //Debug.Log("Target " + target);

        float[] interestMap = new float[directions.Length];



            Vector2 directionToTarget = ((Vector2)target - position).normalized;

     

            for (int i = 0; i < directions.Length; i++)
            {
                float dotProduct = Vector2.Dot(directions[i], directionToTarget);
                float interest = Mathf.Max(0, dotProduct);
            interestMap[i] = interest;  
            Debug.Log($"Direction {i}: {directions[i]} - Interest Value: {interestMap[i]}");
        }
       
        return interestMap;
    }

    public Vector2[] GetDirections() => directions;
}