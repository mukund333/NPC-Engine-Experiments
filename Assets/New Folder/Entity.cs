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

    private float[] dangerMap;//current
    private float[] interestMap;//current
    private float[] combinedMap;

    [SerializeField] private float moveSpeed = 5f;


    private void Start()
    {
        
        dangerMap = new float[compass.GetDirectionCount()];
        interestMap = new float[compass.GetDirectionCount()];
        combinedMap = new float[compass.GetDirectionCount()];


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
        dangerMap = dangerMapCalculator.CalculateDangerMap(obstacles, transform.position);
        mapVisualizer.intersetMap = interestMap;

        //var targets = targetDetectionSystem.GetDetectedTargets();
        var target = targetsDetector.detectedTarget;
        interestMap = interestMapCalculator.CalculateInterestMap(target, transform.position);
        mapVisualizer.dangerMap = dangerMap;



        Vector2 moveDirection = CalculateSteeringDirection();

        Debug.DrawRay(transform.position, moveDirection,Color.magenta);

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

    Vector2 CalculateSteeringDirection()
    {
        
        for (int i = 0; i < compass.GetDirectionCount(); i++)
        {
            combinedMap[i] = interestMap[i] - dangerMap[i];  // Combine interest and danger maps
        }

        // Find the best direction index
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

        // Falloff Calculation: Smooth influence around the best direction
        Vector2 smoothedDirection = Vector2.zero;
        float totalWeight = 0f;
        int falloffRange = 2;  // Number of slots to apply falloff around best slot
        float falloffFactor = 0.5f;  // Controls how quickly influence falls off

        for (int offset = -falloffRange; offset <= falloffRange; offset++)
        {
            int currentIndex = (bestDirectionIndex + offset + combinedMap.Length) % combinedMap.Length;
            float distance = Mathf.Abs(offset);

            // Exponential falloff based on distance
            float weight = combinedMap[currentIndex] * Mathf.Exp(-falloffFactor * distance);
            smoothedDirection += compass.GetDirection(currentIndex) * weight;
            totalWeight += weight;
        }

        return (smoothedDirection / totalWeight).normalized;  // Return the smoothed, normalized direction
    }


    private void Move(Vector2 direction)
    {
        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
    }

   
}