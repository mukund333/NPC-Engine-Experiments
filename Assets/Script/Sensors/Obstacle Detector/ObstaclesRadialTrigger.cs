using System;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesRadialTrigger : MonoBehaviour,IObstacleDetectionSystem
{
    [SerializeField] private float radius = 1f;
    //[SerializeField] private float maxDistance = 5f;
    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] private RaycastHit2D[] hits;
    private readonly int maxHits = 10;
    [SerializeField] private List<Obstacle_Struct> detectedObstacles = new List<Obstacle_Struct>();


    [SerializeField] bool showGizmos;
    public float Radius { get { return radius; } }


   [SerializeField] Collider2D[] nearbyColliders = new Collider2D[20]; // Increase array if needed
  [SerializeField]  List<Obstacle_Struct> persistentObstacles = new List<Obstacle_Struct>();

    void Start()
    {
        hits = new RaycastHit2D[maxHits];
    }

    private void Update()
    {
        Debug.Log("Persistent Obstacles Count: " + detectedObstacles.Count);

    }

    void FixedUpdate()
    {
        UpdateDetection();
    }
    private void UpdateDetection()
    {
        // Clear the detected obstacles list for this frame
        detectedObstacles.Clear();

        // Clear the nearbyColliders array to prevent leftover data from previous frames
        Array.Clear(nearbyColliders, 0, nearbyColliders.Length);

        Vector2 origin = transform.position;
        Vector2 direction = transform.up;
        RaycastHit2D[] hits = new RaycastHit2D[20];

        int numHits = Physics2D.CircleCastNonAlloc(origin, radius, direction, hits, 0, obstacleLayer);

        for (int i = 0; i < numHits; i++)
        {
            nearbyColliders[i] = hits[i].collider;

            foreach (Collider2D collider in nearbyColliders)
            {
                if (collider == null) continue;

                if (collider is PolygonCollider2D polygonCollider)
                {
                    foreach (Vector2 localPoint in polygonCollider.points)
                    {
                        Vector2 worldPoint = polygonCollider.transform.TransformPoint(localPoint);
                        float distanceToPoint = Vector2.Distance(transform.position, worldPoint);

                        Obstacle_Struct newObstacle = new Obstacle_Struct
                        {
                            position = worldPoint,
                            weight = 1f
                        };

                        detectedObstacles.Add(newObstacle);
                      
                    }
                }
            }
        }
    }


    //private void UpdateDetection()
    //{
    //    detectedObstacles.Clear();
    //    Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, radius, obstacleLayer);

    //    foreach (Collider2D collider in nearbyColliders)
    //    {
    //        if (collider is PolygonCollider2D polygonCollider)
    //        {
    //            // Loop through each point in the PolygonCollider
    //            for (int i = 0; i < polygonCollider.points.Length; i++)
    //            {
    //                // Transform the local point to world space
    //                Vector2 worldPoint = polygonCollider.transform.TransformPoint(polygonCollider.points[i]);

    //                // Calculate distance to the point
    //                float distanceToPoint = Vector2.Distance(transform.position, worldPoint);

    //                // Optional: Check if this point is the closest to the object or apply custom logic
    //                Obstacle_Struct newObstacle = new Obstacle_Struct
    //                {
    //                    position = worldPoint,
    //                    weight = 1f / distanceToPoint // Weight can vary by distance
    //                };
    //                detectedObstacles.Add(newObstacle);
    //            }
    //        }
    //    }
    //}

    //private void UpdateDetection()
    //{
    //    detectedObstacles.Clear();
    //    Vector2 direction = transform.up;
    //    Vector2 origin = transform.position;
    //    ////detect area cast
    //    int numHits = Physics2D.CircleCastNonAlloc(
    //        origin,
    //        radius,
    //        direction,
    //        hits,
    //        0,
    //        obstacleLayer
    //    );

    //    ////result of detection
    //    for (int i = 0; i < numHits; i++)
    //    {
    //        Debug.Log(numHits);

    //        ////the exact point of collision
    //        Obstacle_Struct newObstacle = new Obstacle_Struct
    //        {
    //            position = hits[i].point, // Using collision point instead of object center
    //            weight = 1f

    //        };
    //        detectedObstacles.Add(newObstacle);
    //    }
    //}

    public List<Obstacle_Struct> GetDetectedObstacles()
    {
        return detectedObstacles;
    }

    private void OnDrawGizmos()
    {

        if(!showGizmos) return;
        Vector2 direction = transform.up;
        Vector2 origin = transform.position;



        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, radius);
        
        // Draw detected targets
        Gizmos.color = Color.red; // Change color for detected targets
        foreach (var target in detectedObstacles)
        {
            Gizmos.DrawSphere(target.position, 0.2f); // Draw a small sphere at the target position
        }
    }

  
}

