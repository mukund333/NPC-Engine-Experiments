using UnityEngine;
using System.Collections.Generic;



public class TargetsDetector : MonoBehaviour,ITargetDetectionSystem
{
    [SerializeField] private float searchRadius;    // Radius of the circle
    //[SerializeField] private float castDistance;    // Distance to cast the circle
    [SerializeField] private LayerMask targetLayer;

    [SerializeField] private List<Target_Struct> detectedTargets = new List<Target_Struct>();

    private RaycastHit2D[] hits;
    private readonly int maxHits = 10;


    void Start()
    {
        hits = new RaycastHit2D[maxHits];
    }

    void Update()
    {
        UpdateDetection();
    }

    private void UpdateDetection()
    {
        detectedTargets.Clear();
        Vector2 direction = transform.up;
        Vector2 origin = transform.position;


        // detect area cast
        int numHits = Physics2D.CircleCastNonAlloc(
            origin,
            searchRadius,
            direction,
            hits,
            0,
            targetLayer
        );

        // result of detection
        for (int i = 0; i < numHits; i++)
        {
            Target_Struct newObstacleTarget = new Target_Struct
            {
                position = hits[i].collider.transform.position,
             
                weight = DetermineWeight(hits[i].collider)
            };

            detectedTargets.Add(newObstacleTarget);

            // Log the details of the detected target
            Debug.Log($"Detected Target: Position = {newObstacleTarget.position}, Weight = {newObstacleTarget.weight}");
        }
    }

    private float DetermineWeight(Collider2D targetObj)
    {
        if (targetObj.CompareTag("Health"))
            return 5f;
        if (targetObj.CompareTag("Rare"))
            return 3f;
        return 1f;
    }


    public List<Target_Struct> GetDetectedTargets()
    {
        

        return detectedTargets;
    }


    private void OnDrawGizmos()
    {
        Vector2 origin = transform.position;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(origin, searchRadius);

        // Draw detected targets
        Gizmos.color = Color.red; // DarkSeaGreen
        foreach (Target_Struct target in detectedTargets)
        {
            Gizmos.DrawSphere(target.position, 0.1f); // Draw a small sphere at the target position
        }
    }

}
