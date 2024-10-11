using UnityEngine;

public class ObstacleAvoidanceBehavior : IBehavior
{
    #region variables
    public int Priority { get; private set; } = 3; // Higher priority than Arrive
    public InfluenceType InfluenceType => InfluenceType.Danger;
    public float utilityWeight { get; set; } = 1.5f;
    public GearState gearState { get; set; }
    public string Name => "Behavior Name : ObstacleAvoidanceBehavior";

    [SerializeField] private float criticalDistance = 2f;
    [SerializeField] private float threateningDistance = 6f;
    [SerializeField] private float cautiousDistance = 8f;

    private FieldOfView fieldOfView;
    private DetectedObject currentObstacle;
    #endregion

    #region behavior
    public ObstacleAvoidanceBehavior(FieldOfView fov)
    {
        fieldOfView = fov;
    }

    private void UpdateCurrentObstacle()
    {
        currentObstacle = fieldOfView.GetMostThreateningObstacle();
    }
    #endregion

    #region Utility
    public float CalculateUtility(Vector2 entityPosition)
    {
        UpdateCurrentObstacle();
        if (currentObstacle == null || currentObstacle.Target == null) return 0f;

        DangerLevel dangerLevel = GetDangerLevel(currentObstacle.Distance);
        float dangerValue = GetDangerValue(dangerLevel);
        return dangerValue * utilityWeight;
    }
    #endregion

    #region Gear
    public GearState CalculateGears(Vector2 entityPosition)
    {
        UpdateCurrentObstacle();
        if (currentObstacle == null || currentObstacle.Target == null) return GearState.Third;

        if (currentObstacle.Distance <= criticalDistance)
        {
            gearState = GearState.Neutral;
        }
        else if (currentObstacle.Distance <= threateningDistance)
        {
            gearState = GearState.Second;
        }
        else
        {
            gearState = GearState.Third;
        }
        return gearState;
    }
    #endregion

    #region Influence Map
    public float[] CalculateInfluenceMap(Vector2 entityPosition, int numSlots, int falloffRange)
    {
        float[] influenceMap = new float[numSlots];
        UpdateCurrentObstacle();
        if (currentObstacle == null || currentObstacle.Target == null) return influenceMap;

        Vector2 directionToObstacle = currentObstacle.Direction;
        float distance = currentObstacle.Distance;

        if (distance < 0.0001f) return influenceMap;

        float angle = Mathf.Atan2(directionToObstacle.x, directionToObstacle.y) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;
        int slotIndex = Mathf.FloorToInt(angle / (360f / numSlots)) % numSlots;

        DangerLevel dangerLevel = GetDangerLevel(distance);
        float dangerValue = GetDangerValue(dangerLevel);

        for (int i = -falloffRange; i <= falloffRange; i++)
        {
            int index = (slotIndex + i + numSlots) % numSlots;
            float falloff = Mathf.Exp(-(i * i) / (2f * falloffRange));
            influenceMap[index] = dangerValue * falloff;
        }
        return influenceMap;
    }
    #endregion

    #region other helper methods
    private DangerLevel GetDangerLevel(float distance)
    {
        if (distance <= criticalDistance) return DangerLevel.Critical;
        if (distance <= threateningDistance) return DangerLevel.Threatening;
        if (distance <= cautiousDistance) return DangerLevel.Cautious;
        return DangerLevel.Safe;
    }

    private float GetDangerValue(DangerLevel level)
    {
        switch (level)
        {
            case DangerLevel.Critical: return 10.0f;
            case DangerLevel.Threatening: return 0.5f;
            case DangerLevel.Cautious: return 0.25f;
            default: return 0f; // Safe
        }
    }
    #endregion
}