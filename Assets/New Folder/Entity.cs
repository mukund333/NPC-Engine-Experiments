using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObstaclesDetector))]
public class Entity : MonoBehaviour
{

    private readonly Compass compass = Compass.Instance;
    MapVisualizer mapVisualizer;

    private InterestMapCalculator interestMapCalculator;
    private DangerMapCalculator dangerMapCalculator;


    //private ITargetDetectionSystem targetDetectionSystem;
    private IObstacleDetectionSystem obstacleDetectionSystem;
    private TargetsDetector targetsDetector;

    private float[] currentDangerValues;
    private float[] currentIntersetValues;

    [SerializeField] private float moveSpeed = 5f;


    private void Start()
    {
        



        interestMapCalculator = new InterestMapCalculator();
        dangerMapCalculator = new DangerMapCalculator();

        //targetDetectionSystem = GetComponent<ITargetDetectionSystem>();
        obstacleDetectionSystem = GetComponent<ObstaclesDetector>();
        targetsDetector = GetComponent<TargetsDetector>();
         mapVisualizer = GetComponent<MapVisualizer>();

    }

    private void Update()
    {
        var obstacles = obstacleDetectionSystem.GetDetectedObstacles();
        currentDangerValues = dangerMapCalculator.CalculateDangerMap(obstacles, transform.position);
        mapVisualizer.intersetMap = currentIntersetValues;

        //var targets = targetDetectionSystem.GetDetectedTargets();
        var target = targetsDetector.detectedTarget;
        currentIntersetValues = interestMapCalculator.CalculateInterestMap(target, transform.position);
        mapVisualizer.dangerMap = currentDangerValues;


        //Vector2 moveDirection =
        //Vector2 moveDirection = GetBestDirection();
        //Move(moveDirection);
    }

    //private Vector2 GetBestDirection()
    //{
    //    float highestInterest = 0f;
    //    int bestIndex = 0;

    //    for (int i = 0; i < currentDangerValues.Length; i++)
    //    {
    //        if (currentDangerValues[i] > highestInterest)
    //        {
    //            highestInterest = currentDangerValues[i];
    //            bestIndex = i;
    //        }
    //    }

    //    return dangerMapCalculator.GetDirections()[bestIndex];
    //}

    private void Move(Vector2 direction)
    {
        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
    }

   
}