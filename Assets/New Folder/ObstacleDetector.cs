using UnityEngine;

public class ObstacleDetector : MonoBehaviour
{
    [SerializeField] private float radius = 1f;        // Circle radius
    [SerializeField] private float maxDistance = 5f;   // How far to check
    [SerializeField] private LayerMask obstacleLayer;  // Which layers to check

    private RaycastHit2D[] hits;
    private readonly int maxHits = 10;  // Maximum objects to detect

    void Start()
    {
        hits = new RaycastHit2D[maxHits];
    }

    void Update()
    {
        Vector2 direction = transform.up;  // Direction to cast
        Vector2 origin = transform.position;  // Starting point

        int numHits = Physics2D.CircleCastNonAlloc(
            origin,         // Starting point
            radius,        // Circle radius
            direction,     // Direction to cast
            hits,          // Pre-allocated array for results
            maxDistance,   // How far to check
            obstacleLayer  // Which layers to check
        );

        Debug.DrawRay(origin, direction * maxDistance, Color.red);  // Visualization

        // Process all detected hits
        for (int i = 0; i < numHits; i++)
        {
            Debug.Log($"Hit object: {hits[i].collider.name} at distance: {hits[i].distance}");

            // Example: Change color of hit objects to red
            var spriteRenderer = hits[i].collider.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.red;
            }
        }
    }

    // Draw Gizmos to visualize maxDistance and radius in the Scene view
    private void OnDrawGizmos()
    {
        Vector2 direction = transform.up;  // Direction to cast
        Vector2 origin = transform.position;  // Starting point

        // Draw the detection range as a line
        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin, origin + direction * maxDistance);

        // Draw the circle cast as a wire sphere at the starting point
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, radius);

        // Draw the circle cast end point
        Gizmos.DrawWireSphere(origin + direction * maxDistance, radius);
    }
}
