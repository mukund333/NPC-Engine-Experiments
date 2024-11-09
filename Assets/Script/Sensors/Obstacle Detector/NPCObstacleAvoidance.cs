using System.Collections.Generic;
using UnityEngine;

public class NPCObstacleAvoidance : MonoBehaviour
{
    [Header("Detection Settings")]
    public float capsuleRadius = 1f;
    public float capsuleLength = 3f;
    public float overlapRadius = 2f;
    public LayerMask obstacleLayer;
    public float fovAngle = 90f;

    [Header("Gizmo Settings")]
    public bool showGizmos = true;

    private List<Vector2> detectedObstaclePoints = new List<Vector2>();

    private void Update()
    {
        PerformCapsuleCast();
    }

    private void PerformCapsuleCast()
    {
        Vector2 origin = (Vector2)transform.position;
        Vector2 direction = transform.up; // Forward direction of the NPC

        // Use CapsuleCastNonAlloc to detect obstacles in front
        RaycastHit2D[] capsuleHits = new RaycastHit2D[10];

        int hitCount = Physics2D.CapsuleCastNonAlloc(
            origin ,
            new Vector2(capsuleRadius, capsuleLength),
            CapsuleDirection2D.Vertical,
            0,
            direction,
            capsuleHits,       // This array stores the hits, positioned here
            capsuleLength,     // The maximum cast distance
            obstacleLayer
        );

        detectedObstaclePoints.Clear();

        for (int i = 0; i < hitCount; i++)
        {
            RaycastHit2D hit = capsuleHits[i];
            if (IsInFOV(hit.point))
            {
                detectedObstaclePoints.Add(hit.point);

                // Use OverlapCircleAll for detailed analysis if close enough
                if (Vector2.Distance(origin, hit.point) <= overlapRadius)
                {
                    Collider2D[] overlapHits = Physics2D.OverlapCircleAll(hit.point, overlapRadius, obstacleLayer);
                    foreach (Collider2D collider in overlapHits)
                    {
                        if (collider is PolygonCollider2D polygonCollider)
                        {
                            foreach (Vector2 localPoint in polygonCollider.points)
                            {
                                Vector2 worldPoint = polygonCollider.transform.TransformPoint(localPoint);
                                detectedObstaclePoints.Add(worldPoint);
                            }
                        }
                    }
                }
            }
        }
    }



    private bool IsInFOV(Vector2 point)
    {
        Vector2 directionToPoint = point - (Vector2)transform.position;
        float angle = Vector2.Angle(transform.up, directionToPoint);
        return angle <= fovAngle / 2;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        // Draw CapsuleCast gizmo
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + transform.up * (capsuleLength / 2), capsuleRadius);
        Gizmos.DrawWireSphere(transform.position - transform.up * (capsuleLength / 2), capsuleRadius);
        Gizmos.DrawLine(
            transform.position + transform.up * (capsuleLength / 2) + transform.right * capsuleRadius,
            transform.position - transform.up * (capsuleLength / 2) + transform.right * capsuleRadius
        );
        Gizmos.DrawLine(
            transform.position + transform.up * (capsuleLength / 2) - transform.right * capsuleRadius,
            transform.position - transform.up * (capsuleLength / 2) - transform.right * capsuleRadius
        );

        // Draw FOV cone
        Gizmos.color = Color.yellow;
        Vector3 leftBoundary = Quaternion.Euler(0, 0, -fovAngle / 2) * transform.up * capsuleLength;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, fovAngle / 2) * transform.up * capsuleLength;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);

        // Draw detected obstacle points
        Gizmos.color = Color.red;
        foreach (Vector2 point in detectedObstaclePoints)
        {
            Gizmos.DrawSphere(point, 0.1f);
        }

        // Draw OverlapCircleAll detection range
        Gizmos.color = Color.green;
        foreach (Vector2 point in detectedObstaclePoints)
        {
            Gizmos.DrawWireSphere(point, overlapRadius);
        }
    }
}
