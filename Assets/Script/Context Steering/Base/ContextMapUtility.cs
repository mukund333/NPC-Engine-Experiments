using UnityEngine;
// For completeness, also including the ContextMapUtility
public static class ContextMapUtility
{
    public static float[] SmoothContextMap(float[] map, int radius)
    {
        float[] smoothedMap = new float[map.Length];
        for (int i = 0; i < map.Length; i++)
        {
            float total = 0;
            int count = 0;
            for (int j = -radius; j <= radius; j++)
            {
                int index = (i + j + map.Length) % map.Length;
                total += map[index];
                count++;
            }
            smoothedMap[i] = total / count;
        }
        return smoothedMap;
    }
}
