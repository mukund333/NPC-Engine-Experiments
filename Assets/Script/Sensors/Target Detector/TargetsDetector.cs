using UnityEngine;
using System.Collections.Generic;

public class TargetsDetector : MonoBehaviour
{
    [SerializeField] RadialTrigger radialTrigger;
    [SerializeField] private float searchRadius;    // Radius of the circle
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask detectionLayers;

    // Target Status
    private bool canSeeTarget = false;
    [SerializeField] private Vector2 lastKnownPosition = Vector2.positiveInfinity; // Initial "uninitialized" value
    public Vector3 detectedTarget;

    [SerializeField] private bool showGizmos;

    private void Start()
    {
        radialTrigger = GetComponent<RadialTrigger>();
        radialTrigger.radius = searchRadius;
        radialTrigger.target = target;
    }

    void Update()
    {
        UpdateDetection();

        if (lastKnownPosition != Vector2.positiveInfinity) // Only set if lastKnownPosition is valid
        {
            detectedTarget = lastKnownPosition;
        }
    }

    private void UpdateDetection()
    {
        // Primary detection
        if (radialTrigger.inside)
        {
            // Get target direction
            Vector2 direction = target.position - transform.position;
            direction.Normalize();

            // Secondary detection
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, searchRadius, detectionLayers);

            // Result of detection
            if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Target"))
            {
                canSeeTarget = true;
                lastKnownPosition = target.position; // Update last known position
            }
            else if (hit.collider != null)
            {
                if (canSeeTarget) // Just lost sight
                {
                    canSeeTarget = false;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Vector2 origin = transform.position;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(origin, searchRadius);

        // Draw detected targets only if we have a valid position
        if (lastKnownPosition != Vector2.positiveInfinity)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lastKnownPosition, 0.1f); // Draw a small sphere at the last known position
        }
    }
}
