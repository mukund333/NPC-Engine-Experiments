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

    void Start()
    {
        hits = new RaycastHit2D[maxHits];
    }
  

    void FixedUpdate()
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
            0,
            obstacleLayer
        );

        // result of detection
        for (int i = 0; i < numHits; i++)
        {
            //Obstacle_Struct newObstacle = new Obstacle_Struct
            //{
            //    position = hits[i].collider.transform.position,
            //    weight = 1f // All targets equal weight as per requirement
            //};



            //the exact point of collision
            Obstacle_Struct newObstacle = new Obstacle_Struct
            {
                position = hits[i].point, // Using collision point instead of object center
                weight = 1f
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

