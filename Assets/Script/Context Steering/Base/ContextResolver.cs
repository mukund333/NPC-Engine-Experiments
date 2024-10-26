using UnityEngine;

public static class ContextResolver
{


    public static Vector2 ResolveContexts(float[] interestMap, float[] dangerMap, int numSlots)
    {

        //Debug.Log("<color=green>Interest map:</color> " + string.Join(", ", interestMap));
        //Debug.Log("<color=red>Danger map:</color> " + string.Join(", ", dangerMap));


        Vector2 resultDirection = Vector2.zero;
        float anglePerSlot = 360f / numSlots;

        for (int i = 0; i < numSlots; i++)
        {
            float angle = i * anglePerSlot * Mathf.Deg2Rad;
            float weight = interestMap[i] - dangerMap[i];

            resultDirection += new Vector2(
                Mathf.Sin(angle),
                Mathf.Cos(angle)
            ) * weight;
        }

        if (resultDirection.sqrMagnitude > 0.0001f)
        {
            return resultDirection.normalized;
        }
        else
        {
            return Vector2.up; // Default direction when no clear preference
        }
    }


}

