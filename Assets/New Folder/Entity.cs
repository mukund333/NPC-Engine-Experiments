using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObstaclesDetector))]
public class Entity : MonoBehaviour
{
    private InterestMapCalculator interestMapCalculator;
    private DangerMapCalculator dangerMapCalculator;


    private ITargetDetectionSystem targetDetectionSystem;
    private IObstacleDetectionSystem obstacleDetectionSystem;

    private float[] currentDangerValues;
    private float[] currentIntersetValues;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool showDebug = true;
    [SerializeField] private float debugRayLength = 1f;

    private void Start()
    {
        interestMapCalculator = new InterestMapCalculator();
        dangerMapCalculator = new DangerMapCalculator();

        targetDetectionSystem = GetComponent<ITargetDetectionSystem>();
        obstacleDetectionSystem = GetComponent<ObstaclesDetector>();
        

    }

    private void Update()
    {
        var obstacles = obstacleDetectionSystem.GetDetectedObstacles();
        currentDangerValues = dangerMapCalculator.CalculateDangerMap(obstacles, transform.position);


        var targets = targetDetectionSystem.GetDetectedTargets();
        currentIntersetValues = interestMapCalculator.CalculateInterestMap(targets, transform.position);


        Vector2 moveDirection = GetBestDirection();
        //Move(moveDirection);
    }

    private Vector2 GetBestDirection()
    {
        float highestInterest = 0f;
        int bestIndex = 0;

        for (int i = 0; i < currentDangerValues.Length; i++)
        {
            if (currentDangerValues[i] > highestInterest)
            {
                highestInterest = currentDangerValues[i];
                bestIndex = i;
            }
        }

        return dangerMapCalculator.GetDirections()[bestIndex];
    }

    private void Move(Vector2 direction)
    {
        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        //if (!showDebug || currentIntersetValues == null || interestMapCalculator == null) return;

        //Vector2[] dirs = interestMapCalculator.GetDirections();

        //for (int i = 0; i < currentIntersetValues.Length; i++)
        //{
        //    float length = currentIntersetValues[i] * debugRayLength;
        //    Vector3 direction = dirs[i] * length;
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawRay(transform.position, direction);
        //}

        if (!showDebug || currentDangerValues == null || dangerMapCalculator == null) return;


        Vector2[] dirs2 = dangerMapCalculator.GetDirections();
        for (int i = 0; i < currentDangerValues.Length; i++)
        {
            float length = currentDangerValues[i] * debugRayLength;
            Vector3 direction = dirs2[i] * length;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, direction);
        }

    }
}