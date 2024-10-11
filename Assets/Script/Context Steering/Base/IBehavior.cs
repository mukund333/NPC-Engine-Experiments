using UnityEngine;
public interface IBehavior
{
    string Name { get; }
    int Priority { get; } // Priority value to break ties
    InfluenceType InfluenceType { get; }
    float utilityWeight { get; set; }

    GearState gearState { get; set; }

    float CalculateUtility(Vector2 entityPosition);

    GearState CalculateGears(Vector2 entityPosition);

    float[] CalculateInfluenceMap(Vector2 entityPosition, int numSlots, int falloffRange);
}