using UnityEngine;
using System.Collections.Generic;

public class ObstaclesDetector : MonoBehaviour, IObstacleDetectionSystem
{
    [SerializeField] private float radius = 1f;
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private LayerMask obstacleLayer;

    private RaycastHit2D[] hits;
    private readonly int maxHits = 10;
    [SerializeField] private List<Obstacle_Struct> detectedObstacles = new List<Obstacle_Struct>();

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
        detectedObstacles.Clear();
        Vector2 direction = transform.up;
        Vector2 origin = transform.position;
        // detect area cast
        int numHits = Physics2D.CircleCastNonAlloc(
            origin,
            radius,
            direction,
            hits,
            maxDistance,
            obstacleLayer
        );

        // result of detection
        for (int i = 0; i < numHits; i++)
        {
            Obstacle_Struct newObstacle = new Obstacle_Struct
            {
                position = hits[i].collider.transform.position,
                weight = 1f // All targets equal weight as per requirement
            };
            detectedObstacles.Add(newObstacle);
        }
    }
 
    public List<Obstacle_Struct> GetDetectedObstacles()
    {
        return detectedObstacles;
    }

    private void OnDrawGizmos()
    {
        Vector2 direction = transform.up;
        Vector2 origin = transform.position;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(origin, origin + direction * maxDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, radius);
        Gizmos.DrawWireSphere(origin + direction * maxDistance, radius);

        // Draw detected targets
        Gizmos.color = Color.red; // Change color for detected targets
        foreach (Obstacle_Struct target in detectedObstacles)
        {
            Gizmos.DrawSphere(target.position, 0.1f); // Draw a small sphere at the target position
        }
    }

    public List<Target_Struct> GetDetectedTargets()
    {
        throw new System.NotImplementedException();
    }
}

