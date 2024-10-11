using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class EntityBehaviorManager : MonoBehaviour
{
    public int numSlots = 36;
    public int falloffRange = 3;
    public int smoothingRadius = 1;
    public float[] combineInterestMap;
    public float[] combineDangerMap;

    public float[] interestMap; // single bahevior map calculation
    public float[] dangerMap; // single bahevior map calculation


    private Vector2 previousBestDirection;
    public float directionSmoothingFactor = 0.5f;


    private Dictionary<IBehavior, float> behaviorUtilities = new Dictionary<IBehavior, float>();
    public List<IBehavior> allBehaviors = new List<IBehavior>();

    #region movement engine
    IMovementEngineController movementEngineController;


    [SerializeField] private Vector2 bestDirection;

    [SerializeField]
    FieldOfView fieldOfView;
    #endregion
    void Start()
    {
        movementEngineController = GetComponent<IMovementEngineController>();

        combineInterestMap = new float[numSlots];
        combineDangerMap = new float[numSlots];


        // Get the FieldOfView component
         fieldOfView = GetComponentInChildren<FieldOfView>();



        allBehaviors.Add(new ArriveBehavior());
        allBehaviors.Add(new ObstacleAvoidanceBehavior(fieldOfView));
    }

 

    void FixedUpdate()
    {
        //final direction
        DirectionCalculation();
        movementEngineController.SetDirection(bestDirection);

        //top gear
        UpdateBehaviorUtilities();
        IBehavior bestBehavior = GetBestBehavior();
        if (bestBehavior != null)
        {
            //Debug.Log(bestBehavior.Name);
            SetGear(bestBehavior.CalculateGears(transform.position));
        }

    }


    #region Calculate Final Direction
    void DirectionCalculation()
    {  // Reset the combineInterestMap
        for (int i = 0; i < combineInterestMap.Length; i++)
        {
            combineInterestMap[i] = 0f;
        }

        for (int i = 0; i < combineDangerMap.Length; i++)
        {
            combineDangerMap[i] = 0f;
        }
        foreach (var behavior in allBehaviors)
        {
            ExecuteBehavior(behavior);
        }
    }

    private void ExecuteBehavior(IBehavior behavior)
    {
        float[] behaviorMap;

        switch (behavior.InfluenceType)
        {
            case InfluenceType.Interest:
                behaviorMap = behavior.CalculateInfluenceMap(transform.position, numSlots, falloffRange);
                behaviorMap = ContextMapUtility.SmoothContextMap(behaviorMap, smoothingRadius);
                NormalizeMap(behaviorMap);
                AddContextMap(combineInterestMap, behaviorMap);
                break;
            case InfluenceType.Danger:
                behaviorMap = behavior.CalculateInfluenceMap(transform.position, numSlots, falloffRange);
                behaviorMap = ContextMapUtility.SmoothContextMap(behaviorMap, smoothingRadius);
                NormalizeMap(behaviorMap);
                AddContextMap(combineDangerMap, behaviorMap);
                break;
        }

        Vector2 newBestDirection = ContextResolver.ResolveContexts(combineInterestMap, combineDangerMap, numSlots);

        // Smooth the direction change
        if (previousBestDirection != Vector2.zero)
        {
            bestDirection = Vector2.Lerp(previousBestDirection, newBestDirection, directionSmoothingFactor);
        }
        else
        {
            bestDirection = newBestDirection;
        }

        previousBestDirection = bestDirection;
    }

    private void NormalizeMap(float[] map)
    {
        float maxValue = Mathf.Max(map);
        if (maxValue > 0)
        {
            for (int i = 0; i < map.Length; i++)
            {
                map[i] /= maxValue;
            }
        }
    }
    // Utility function to add one context map to another
    void AddContextMap(float[] baseMap, float[] additionalMap)
    {
        for (int i = 0; i < baseMap.Length; i++)
        {
            baseMap[i] += additionalMap[i];
        }
    }

    #endregion


    #region Calculate Gears
    void UpdateBehaviorUtilities()
    {
        foreach (var behavior in allBehaviors)
        {
            behaviorUtilities[behavior] = behavior.CalculateUtility(transform.position);
        }
    }


    IBehavior GetBestBehavior()
    {
        float highestUtility = float.MinValue;
        IBehavior bestBehavior = null;
        foreach (var entry in behaviorUtilities)
        {
            if (entry.Value > highestUtility)
            {
                highestUtility = entry.Value;
                bestBehavior = entry.Key;
            }
        }
        return bestBehavior;
    }

    void SetGear(GearState gear)
    {
        //////
        /// movement  gear  algo
        //////

        movementEngineController.SetTargetGear(gear);
    }



    #endregion



    #region Visual test
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        // Draw interest map
        DrawInfluenceMap(combineInterestMap, Color.green);

        // Draw danger map
        DrawInfluenceMap(combineDangerMap, Color.red);

        // Draw best direction
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + bestDirection * 2f);
    }

    private void DrawInfluenceMap(float[] map, Color baseColor)
    {
        float anglePerSlot = 360f / numSlots;
        for (int i = 0; i < numSlots; i++)
        {
            float angle = i * anglePerSlot * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
            Color color = baseColor * map[i];
            color.a = 0.5f;
            Gizmos.color = color;
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + direction * map[i]);
        }
    }
    #endregion

}
