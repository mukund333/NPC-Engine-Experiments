using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(EntityBehaviorManager_))]
public class DebugVisualizer_ : MonoBehaviour
{
    public Color interestColor = Color.green;     // Color for visualizing interest map
    public Color dangerColor = Color.red;         // Color for visualizing danger map
    public Color finalDirectionColor = Color.blue; // Color for visualizing final direction
    public float lineLength = 1f;                 // Length of the visualization lines
    public bool visualizeInterestMap = true;      // Toggle to visualize the interest map
    public bool visualizeDangerMap = true;        // Toggle to visualize the danger map
    public bool visualizeFinalDirection = true;   // Toggle to visualize the final direction

    private EntityBehaviorManager_ behaviorManager;
    private Vector2 finalDirection;

    void Start()
    {
        behaviorManager = GetComponent<EntityBehaviorManager_>();
    }

    void Update()
    {
        if (behaviorManager == null) return;

        // Calculate the interest and danger maps
        float[] interestMap = new float[behaviorManager.numSlots];
        float[] dangerMap = new float[behaviorManager.numSlots];

        foreach (IBehavior_ behavior in behaviorManager.Behaviors)
        {
            float[] map = behavior.CalculateInfluenceMap(transform.position, behaviorManager.numSlots, behaviorManager.falloffRange);

            if (behavior is SeekBehavior_)
            {
                CombineMaps(interestMap, map, behavior.Weight);
            }
            else if (behavior is AvoidBehavior_)
            {
                CombineMaps(dangerMap, map, behavior.Weight);
            }
        }

        // Smooth the maps
        interestMap = ContextMapUtility_.SmoothContextMap(interestMap, behaviorManager.smoothingRadius);
        dangerMap = ContextMapUtility_.SmoothContextMap(dangerMap, behaviorManager.smoothingRadius);

        // Resolve final direction
        finalDirection = ContextResolver_.ResolveContexts(interestMap, dangerMap, behaviorManager.numSlots);

        // Draw visualization in the scene
        VisualizeMaps(interestMap, dangerMap);
    }

    void VisualizeMaps(float[] interestMap, float[] dangerMap)
    {
        float anglePerSlot = 360f / behaviorManager.numSlots;
        Vector2 position = transform.position;

        // Visualize Interest Map
        if (visualizeInterestMap)
        {
            for (int i = 0; i < interestMap.Length; i++)
            {
                float angle = i * anglePerSlot;
                Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;
                Debug.DrawLine(position, position + direction * interestMap[i] * lineLength, interestColor);
            }
        }

        // Visualize Danger Map
        if (visualizeDangerMap)
        {
            for (int i = 0; i < dangerMap.Length; i++)
            {
                float angle = i * anglePerSlot;
                Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;
                Debug.DrawLine(position, position + direction * dangerMap[i] * lineLength, dangerColor);
            }
        }

        // Visualize Final Direction
        if (visualizeFinalDirection)
        {
            Debug.DrawLine(position, position + finalDirection * lineLength, finalDirectionColor);
        }
    }

    // Helper method to combine maps with weighting
    private void CombineMaps(float[] baseMap, float[] additionalMap, float weight)
    {
        for (int i = 0; i < baseMap.Length; i++)
        {
            baseMap[i] += additionalMap[i] * weight;
        }
    }
}
