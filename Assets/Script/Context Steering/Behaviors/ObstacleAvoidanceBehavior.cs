using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ObstacleAvoidanceBehavior : IBehavior
{
    public string Name => "Obstacle Avoidance";
    public int Priority { get; private set; }
    public InfluenceType InfluenceType => InfluenceType.Danger;
    public float utilityWeight { get; set; }
    public GearState gearState { get; set; }

    private float avoidanceRadius;
    private BoxCast2DDectection boxCaster;

    public ObstacleAvoidanceBehavior(int priority, float avoidanceRadius, BoxCast2DDectection boxCaster)
    {
        this.Priority = priority;
        this.avoidanceRadius = avoidanceRadius;
        this.boxCaster = boxCaster;
    }

    public float CalculateUtility(Vector2 entityPosition)
    {
        RaycastHit2D hitInfo = boxCaster.PerformBoxCast();

        if (hitInfo.collider != null)
        {
            float distance = hitInfo.distance;
            return Mathf.Clamp01(1 - (distance / avoidanceRadius)) * utilityWeight;
        }
        return 0f;
    }

    public float[] CalculateInfluenceMap(Vector2 entityPosition, int numSlots, int falloffRange)
    {
        float[] influenceMap = new float[numSlots];
        RaycastHit2D hit = boxCaster.PerformBoxCast();

        if (hit.collider != null)
        {
            Vector2 hitDirection = (hit.point - (Vector2)boxCaster.transform.position).normalized;
            int slotIndex = GetSlotIndex(hitDirection, numSlots);
            float distance = hit.distance;
            float influenceValue = Mathf.Clamp01(1 - (distance / avoidanceRadius));

            for (int i = 0; i < numSlots; i++)
            {
                float falloff = Mathf.Max(0, 1 - Mathf.Abs(slotIndex - i) / (float)falloffRange);
                influenceMap[i] += influenceValue * falloff;
            }
        }
        //Debug.Log("Influence map: " + string.Join(", ", influenceMap));
        return influenceMap;
    }

    public GearState CalculateGears(Vector2 entityPosition)
    {
        float utility = CalculateUtility(entityPosition);

        if (utility > 0)
            return GearState.First;
        //else if (utility > 0.4f)
        //    return GearState.Neutral;
        else
            return GearState.Third;
    }

    private int GetSlotIndex(Vector2 direction, int numSlots)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float normalizedAngle = (angle + 360) % 360;
        return Mathf.FloorToInt(normalizedAngle / (360f / numSlots));
    }
}