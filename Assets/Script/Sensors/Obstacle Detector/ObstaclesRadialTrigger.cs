using System;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesRadialTrigger : MonoBehaviour,IObstacleDetectionSystem
{
    [SerializeField] private float radius = 1f;
    //[SerializeField] private float maxDistance = 5f;
    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] private RaycastHit2D[] hits;
    private readonly int maxHits = 25;//near GameObject allies,obstacles


    [SerializeField] private List<Obstacle_Struct> detectedObstacles = new List<Obstacle_Struct>();


    [SerializeField] bool showGizmos;
    public float Radius { get { return radius; } }


   [SerializeField] Collider2D[] nearbyObstaclesColliders = new Collider2D[20]; // Increase array if needed

    // Define the layer you want to filter by
    int obstacleLayerIndex ; // Replace "ObstacleLayerName" with your actual layer name

    private void Awake()
    {
        obstacleLayerIndex = LayerMask.NameToLayer("Obstacle");
        hits = new RaycastHit2D[maxHits];
    }
   
   
    void FixedUpdate()
    {
        UpdateDetection();
        CalculateObstacles();
    }
    private void UpdateDetection()
    {
        // Clear the detected obstacles list for this frame
        detectedObstacles.Clear();

        // Clear the nearbyColliders array to prevent leftover data from previous frames
        Array.Clear(nearbyObstaclesColliders, 0, nearbyObstaclesColliders.Length);
       

        Vector2 origin = transform.position;
        Vector2 direction = transform.up;


        int numHits = Physics2D.CircleCastNonAlloc(origin, radius, direction, hits, 0, obstacleLayer);

        for (int i = 0; i < numHits; i++)
        {

            Collider2D hitCollider = hits[i].collider;
            if (hitCollider.gameObject.layer == obstacleLayerIndex)
            { 
           
                nearbyObstaclesColliders[i] = hits[i].collider;
            }
        }
    }

    private void CalculateObstacles()
    {
       
        foreach (PolygonCollider2D collider in nearbyObstaclesColliders)
        {
            if (collider == null) continue;

          
                foreach (Vector2 localPoint in collider.points)
                {
                    Vector2 worldPoint = collider.transform.TransformPoint(localPoint);
                    float distanceToPoint = Vector2.Distance(transform.position, worldPoint);

                     if(distanceToPoint <= Radius)
                    {

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
        Gizmos.color = Color.magenta; // Change color for detected targets
        foreach (var target in detectedObstacles)
        {
            Gizmos.DrawSphere(target.position, 0.2f); // Draw a small sphere at the target position
        }
    }

  
}

