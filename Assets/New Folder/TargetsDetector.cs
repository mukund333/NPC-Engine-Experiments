using UnityEngine;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;



public class TargetsDetector : MonoBehaviour
{
    [SerializeField] RadialTrigger radialTrigger;

    [SerializeField] private float searchRadius;    // Radius of the circle

    [SerializeField] private Transform target;

    [SerializeField] private LayerMask detectionLayers;

    // Target Status
    private bool canSeeTarget = false;
    [SerializeField] private Vector2 lastKnownPosition;


    public Vector3 detectedTarget;
    private void Start()
    {
  
        radialTrigger = GetComponent<RadialTrigger>();
        radialTrigger.radius = searchRadius;
        radialTrigger.target = target;
    }

    void Update()
    {
        UpdateDetection();
        if (lastKnownPosition != null)
            detectedTarget = lastKnownPosition;
     
    }

    private void UpdateDetection()
    {
        //primary detection
        if(radialTrigger.inside)
        {
            //get target direction
            Vector2 direction = target.position - transform.position;
            direction.Normalize();

            //secondary detection
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction,searchRadius,detectionLayers);

            //result of detection
            //Make sure that the collider we see is on the "Target" layer
            if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Target"))
            {

                canSeeTarget = true;
                lastKnownPosition = target.position;



                Debug.DrawRay(transform.position, direction * searchRadius, Color.magenta);
            }
            else if(hit.collider != null )
            {
                if (canSeeTarget) // Just lost sight
                {
                    // Store last position when losing sight
                    canSeeTarget = false;
                }
                //Debug.DrawRay(transform.position, lastKnownPosition * searchRadius, Color.red);

            }

        }
    }

    //public Target_Struct GetDetectedTarget()
    //{


    //    return detectedTarget;
    //}
    private void OnDrawGizmos()
    {
        Vector2 origin = transform.position;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(origin, searchRadius);

        // Draw detected targets
        Gizmos.color = Color.red; // DarkSeaGreen
       
        Gizmos.DrawSphere(lastKnownPosition, 0.1f); // Draw a small sphere at the target position
       
    }

   
}
