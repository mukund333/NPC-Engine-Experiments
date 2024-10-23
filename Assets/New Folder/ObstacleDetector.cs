

// ObstacleDetector.cs
using UnityEngine;
using System.Collections.Generic;

public class ObstacleDetector : MonoBehaviour, IDetectionSystem
{
    [SerializeField] private float radius = 1f;
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private LayerMask obstacleLayer;

    private RaycastHit2D[] hits;
    private readonly int maxHits = 10;
    private List<Target> detectedTargets = new List<Target>();

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

        int numHits = Physics2D.CircleCastNonAlloc(
            origin,
            radius,
            direction,
            hits,
            maxDistance,
            obstacleLayer
        );

        for (int i = 0; i < numHits; i++)
        {
            Target newTarget = new Target
            {
                position = hits[i].collider.transform.position,
                weight = 1f // All targets equal weight as per requirement
            };
            detectedTargets.Add(newTarget);
        }
    }

    public List<Target> GetDetectedTargets()
    {
        return detectedTargets;
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
    }
}

