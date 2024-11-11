using System.Collections.Generic;
using UnityEngine;

public class ContextSteeringManager : MonoBehaviour
{
    #region variables
    private readonly Compass compass = Compass.Instance;
    MapVisualizer mapVisualizer;

    private InterestMapCalculator interestMapCalculator;
    private DangerMapCalculator dangerMapCalculator;


    //private ITargetDetectionSystem targetDetectionSystem;
    //private IObstacleDetectionSystem obstacleDetectionSystem;
    private FieldOfView fieldOfView;
    private TargetsDetector targetsDetector;

   [SerializeField] private float[] dangerMap;//current
    [SerializeField] private float[] interestMap;//current
    [SerializeField] private float[] combinedMap;

    #endregion

    private void Start()
    {
        
        dangerMap = new float[compass.GetDirectionCount()];
        interestMap = new float[compass.GetDirectionCount()];
        combinedMap = new float[compass.GetDirectionCount()];


        interestMapCalculator = new InterestMapCalculator();
        dangerMapCalculator = new DangerMapCalculator();

        //targetDetectionSystem = GetComponent<ITargetDetectionSystem>();
        //obstacleDetectionSystem = GetComponent<IObstacleDetectionSystem>();
        fieldOfView = GetComponent<FieldOfView>();
        targetsDetector = GetComponent<TargetsDetector>();
         mapVisualizer = GetComponent<MapVisualizer>();

    }

    private void Update()
    {
        var obstacles = fieldOfView.GetDetectedObstaclesPositions();
        dangerMap = dangerMapCalculator.CalculateDangerMap(obstacles, transform.position);
        mapVisualizer.intersetMap = interestMap;

        //var targets = targetDetectionSystem.GetDetectedTargets();
        var target = targetsDetector.detectedTarget;
        interestMap = interestMapCalculator.CalculateInterestMap(target, transform.position);
        mapVisualizer.dangerMap = dangerMap;

        Debug.DrawRay(transform.position, GetBestDirection(), Color.magenta);


    }

   public Vector2 GetBestDirection()
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


        //return compass.GetDirection(bestDirectionIndex);

        //Falloff Calculation: Smooth influence around the best direction
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

    }