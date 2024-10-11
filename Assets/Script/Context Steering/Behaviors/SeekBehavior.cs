using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/*
    entityPosition mean trasnsfrom pos
*/

public class SeekBehavior :IBehavior
{
    public int Priority { get; private set; } = 2;

    public InfluenceType InfluenceType => InfluenceType.Interest;

    public float utilityWeight { get; set; } = 1.0f;
    public GearState gearState { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public string Name => throw new System.NotImplementedException();

    private Transform target;


    public SeekBehavior()
    {
        target = GameObject.FindWithTag("Target").transform;
    }





    public float CalculateUtility(Vector2 entityPosition)
    {
        float distance = Vector2.Distance(entityPosition, target.position);
        // Apply weight to the utility
        return distance > 2f ? utilityWeight : 0.0f; // Multiply utility by weight
    }





    public float[] CalculateInfluenceMap(Vector2 entityPosition, int numSlots, int falloffRange)
    {
        float[] influenceMap = new float[numSlots];
        Vector2 direction = ((Vector2)target.position - entityPosition).normalized;

        float angle = Vector2.SignedAngle(Vector2.up, direction);
        if (angle < 0) angle += 360f;

        int slotIndex = Mathf.RoundToInt(angle / (360f / numSlots)) % numSlots;
        float baseValue = 1.0f;

        for (int i = -falloffRange; i <= falloffRange; i++)
        {
            int index = (slotIndex + i + numSlots) % numSlots;
            float falloff = 1f / (Mathf.Abs(i) + 1);
            influenceMap[index] += baseValue * falloff;
        }

        return influenceMap;
    }

    public GearState CalculateGears(Vector2 entityPosition)
    {
        return GearState.Third;//max speed to reach the target
    }
}
