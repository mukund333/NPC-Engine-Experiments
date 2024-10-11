using UnityEngine;

public interface IBehavior_
{
    // Method to calculate the influence map for the behavior
    float[] CalculateInfluenceMap(Vector2 entityPosition, int numSlots, int falloffRange);

    // Property to define the weight of the behavior's influence
    float Weight { get; }
}
