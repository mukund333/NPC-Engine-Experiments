using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    private Vector2 fovDirection = Vector2.up;

    [SerializeField] Transform threat;

    [Range(1, 180)]
    public float viewAngle;

    //Gizmos params
    public bool isGizmo;
    public float viewRadius;

    [SerializeField]ObstaclesRadialTrigger obstaclesRadialTrigger;

    [SerializeField] private List<Obstacle_Struct> detected_FOV_Obstacles = new List<Obstacle_Struct>();



    private void Awake()
    {
        viewAngle = 90f;

        obstaclesRadialTrigger = GetComponent<ObstaclesRadialTrigger>();
        viewRadius = obstaclesRadialTrigger.Radius;
    }

    private void Update()
    {
        Detection(obstaclesRadialTrigger.GetDetectedObstacles());
    }


    private void Detection(List<Obstacle_Struct> detectedObstacles)
    {
        if(detectedObstacles.Count == 0) { return; }

        detected_FOV_Obstacles.Clear();

        foreach(Obstacle_Struct obstacle in detectedObstacles)
        {




           float dotProduct = GetDotProduct(obstacle);
           if( IsThreatWithinViewAngle(viewAngle, dotProduct) && IsThreatWithinRadius(obstacle))
                detected_FOV_Obstacles.Add(obstacle);
        }

    }
    private float GetDotProduct(Obstacle_Struct threat)
    {
        if (threat == null)
        {
            Debug.LogError("No target  set for FieldOfView detection!");
        }

        Vector2 directionToTarget = (threat.position - (Vector2) transform.position).normalized;
        float dotProduct = Vector2.Dot(directionToTarget, fovDirection);
        return dotProduct;
    }

    private bool IsThreatWithinViewAngle(float viewAngle, float dotProduct)
    {
        return dotProduct > Mathf.Cos(viewAngle * Mathf.Deg2Rad / 2f);
    }

    private bool IsThreatWithinRadius(Obstacle_Struct threat)
    {
      float distance =  Vector2.Distance(threat.position, transform.position);

        return distance<=viewRadius;
    }


    public List<Obstacle_Struct> GetDetectedObstaclesPositions()
    {
        return detected_FOV_Obstacles;
    }

    private void OnDrawGizmos()
    {
        if (!isGizmo )
        {
            return;
        }

        if (Application.isPlaying)
        {
            // Calculate the center of the FOV arc
            Vector3 center = transform.position;

            // Calculate the start and end angles of the FOV arc
            float startAngle = Mathf.Atan2(fovDirection.y, fovDirection.x) * Mathf.Rad2Deg - viewAngle / 2f;
            float endAngle = startAngle + viewAngle;

            // Draw a wireframe sphere to help visualize the arc
            //Gizmos.color = Color.green;
            //Gizmos.DrawWireSphere(center, viewRadius);

            if(detected_FOV_Obstacles.Count > 0) 
                Gizmos.color = Color.red;
            else Gizmos.color = Color.green;


            // Draw the arc using rays
            int numSegments = 2; // Reduce the number of segments
            float segmentAngle = (endAngle - startAngle) / numSegments;
            for (int i = 0; i <= numSegments; i++)
            {
                float a = startAngle + i * segmentAngle;
                Vector3 direction = new Vector3(Mathf.Cos(a * Mathf.Deg2Rad), Mathf.Sin(a * Mathf.Deg2Rad), 0);
                Gizmos.DrawRay(center, direction * viewRadius);
            }




            // Draw detected targets
            Gizmos.color = Color.red; // Change color for detected targets
            if (detected_FOV_Obstacles.Count>0)
            {

                foreach (var target in detected_FOV_Obstacles)
                {
                    Gizmos.DrawSphere(target.position, 0.2f); // Draw a small sphere at the target position
                }
            }
        }
    }

}
