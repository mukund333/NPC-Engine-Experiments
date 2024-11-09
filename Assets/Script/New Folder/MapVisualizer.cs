using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapVisualizer : MonoBehaviour
{
    private readonly Compass compass = Compass.Instance;
    [SerializeField] private bool showDebug = true;
    [SerializeField] private float debugRayLength = 1f;

    public float[] dangerMap;
    public float[] intersetMap;


    private void Start()
    {
        dangerMap = new float[compass.GetDirectionCount()];
        intersetMap = new float[compass.GetDirectionCount()];
    }


    public void DrawMaps(float[] mapValues,Color color)
    { 
        for (int i = 0; i < compass.GetDirectionCount(); i++)
        {
            float length = mapValues[i] * debugRayLength;
            Vector3 direction = compass.GetDirection(i) * length;
            Gizmos.color = color;
            Gizmos.DrawRay(transform.position, direction);
        }
    }

    private void OnDrawGizmos()
    {

        if (!showDebug ) return;

        if (!Application.isPlaying) return; 

        if ( intersetMap != null)
            DrawMaps(intersetMap,Color.green);

        if (dangerMap == null ) return;
        DrawMaps(dangerMap,Color.red);
       

    }


}
