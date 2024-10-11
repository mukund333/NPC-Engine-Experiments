using UnityEngine;

public class ArriveBehavior : IBehavior
{
    public int Priority { get; private set; } = 2;
    public InfluenceType InfluenceType => InfluenceType.Interest;
    public float utilityWeight { get; set; } = 1.0f;
    public GearState gearState { get; set; }
    public string Name => "Behavior Name : ArriveBehavior";

    public Transform target { get; private set; }  // Made public for debugging
    [SerializeField] float playerProximetyDistance;
    [SerializeField] float arriveDistance;

    public ArriveBehavior()
    {
        target = GameObject.FindWithTag("Player").transform;
        playerProximetyDistance = 15f;
        arriveDistance = 5f;
    }

    public float CalculateUtility(Vector2 entityPosition)
    {
        float distance = Vector2.Distance(entityPosition, target.position);

        // Apply weight to the utility
        return distance > 2f ? utilityWeight : 0.0f;
    }
   
    
    
    
    
    public GearState CalculateGears(Vector2 entityPosition)
    {
        float distance = Vector2.Distance(entityPosition, target.position);

        if (distance <= arriveDistance)
        {
            gearState = GearState.Neutral; // Vehicle stopped
        }
        else if (distance < playerProximetyDistance && distance > arriveDistance)
        {
            gearState = GearState.First;
        }
        else
        {
            gearState = GearState.Third;
        }

        return gearState;
    }

    
    
    
    
    public float[] CalculateInfluenceMap(Vector2 entityPosition, int numSlots, int falloffRange)
    {
        float[] influenceMap = new float[numSlots];
        Vector2 directionToTarget = (Vector2)target.position - entityPosition;

        // Check for zero distance to prevent normalization of zero vector
        if (directionToTarget.sqrMagnitude < 0.0001f)
        {
            return influenceMap;
        }

        directionToTarget.Normalize();

        // Calculate angle using Atan2
        float angle = Mathf.Atan2(directionToTarget.x, directionToTarget.y) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;

        int slotIndex = Mathf.FloorToInt(angle / (360f / numSlots)) % numSlots;

        // Use Gaussian-like falloff for smoother transitions
        for (int i = -falloffRange; i <= falloffRange; i++)
        {
            int index = (slotIndex + i + numSlots) % numSlots;
            float falloff = Mathf.Exp(-(i * i) / (2f * falloffRange));
            influenceMap[index] = falloff;
        }

        return influenceMap;
    }

}
