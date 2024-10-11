using UnityEngine;
using System.Collections.Generic;

public class EntityBehaviorManager_ : MonoBehaviour
{
    public float moveSpeed = 2f;           // Speed at which the entity moves
    public int numSlots = 36;              // Number of slots in the context map (resolution)
    public int falloffRange = 3;           // Range for applying falloff around target/danger
    public int smoothingRadius = 1;        // Radius for smoothing the context map

    // Make the behaviors list accessible from outside
    public List<IBehavior_> Behaviors { get; private set; } = new List<IBehavior_>();

    void Start()
    {
        // Initialize and add behaviors
        var seekBehavior = new SeekBehavior_();
        seekBehavior.AddTarget(new Vector2(10, 10));  // Example target
        Behaviors.Add(seekBehavior);

        var avoidBehavior = new AvoidBehavior_();
        avoidBehavior.AddObstacle(new Vector2(5, 5)); // Example obstacle
        Behaviors.Add(avoidBehavior);
    }

    void Update()
    {
        float[] combinedInterestMap = new float[numSlots]; // Initialize interest map
        float[] combinedDangerMap = new float[numSlots];   // Initialize danger map

        // Calculate influence maps from all behaviors
        foreach (var behavior in Behaviors)
        {
            float[] map = behavior.CalculateInfluenceMap(transform.position, numSlots, falloffRange);

            if (behavior is SeekBehavior_)
            {
                CombineMaps(combinedInterestMap, map, behavior.Weight);
            }
            else if (behavior is AvoidBehavior_)
            {
                CombineMaps(combinedDangerMap, map, behavior.Weight);
            }
        }

        // Smooth the context maps
        combinedInterestMap = ContextMapUtility_.SmoothContextMap(combinedInterestMap, smoothingRadius);
        combinedDangerMap = ContextMapUtility_.SmoothContextMap(combinedDangerMap, smoothingRadius);

        // Resolve final direction
        Vector2 bestDirection = ContextResolver_.ResolveContexts(combinedInterestMap, combinedDangerMap, numSlots);

        // Move the entity in the resolved direction
        transform.position += (Vector3)(bestDirection * moveSpeed * Time.deltaTime);
    }

    // Helper method to combine context maps with weighting
    private void CombineMaps(float[] baseMap, float[] additionalMap, float weight)
    {
        for (int i = 0; i < baseMap.Length; i++)
        {
            baseMap[i] += additionalMap[i] * weight;
        }
    }
}
