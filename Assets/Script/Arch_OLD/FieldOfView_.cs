using System.Collections.Generic;
using UnityEngine;

public enum FOVDirection_
{
    Forward,
    Backward,
    Left,
    Right,
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
}
[RequireComponent(typeof(CircleCollider2D))]
public class FieldOfView_ : MonoBehaviour
{
    [Range(1,180)]
    public float viewAngle;
    public float viewRadius;
  
    public LayerMask[] targetLayers; // Array of layer masks
    public Color gizmoColor = Color.green;
    public bool isGizmo;

    public delegate void PlayerDetectedHandler(GameObject player);
    public event PlayerDetectedHandler OnPlayerDetected;

    private bool playerDetected = false;


    [System.Serializable]
    public class DetectedObject
    {
        public GameObject target;
        public float distance;
        public float velocity;
        public Vector2 direction;
    }

    [SerializeField] private List<DetectedObject> detectedObjects = new List<DetectedObject>();
    private Vector2 fovDirection;
    public FOVDirection_ fovDirectionType = FOVDirection_.Forward;

    private HashSet<Collider2D> targetsInViewRadius = new HashSet<Collider2D>();
    private CircleCollider2D triggerCollider;

    private void Start()
    {
        // Ensure targetLayers is set
        if (targetLayers == null || targetLayers.Length == 0)
        {
            Debug.LogError("No target layers set for FieldOfView detection!", gameObject);
        }
        viewAngle = 45f;
        viewRadius = 3f;
        isGizmo = true;
        // Create a trigger collider to detect objects entering the view radius
        triggerCollider = gameObject.GetComponent<CircleCollider2D>();
        triggerCollider.radius = viewRadius;
        triggerCollider.isTrigger = true;

    }

    private void FixedUpdate()
    {
        // Update the fovDirection based on the GameObject's rotation
        fovDirection = GetFOVDirection(fovDirectionType);

        // Check for targets in the field of view
        detectedObjects.Clear();

        foreach (var target in targetsInViewRadius)
        {
            Vector2 directionToTarget = (target.transform.position - transform.position).normalized;
            float dotProduct = Vector2.Dot(directionToTarget, fovDirection);

            if (dotProduct > Mathf.Cos(viewAngle * Mathf.Deg2Rad / 2f))
            {
                // Target is in the field of view
                DetectedObject detectedObject = new DetectedObject();
                detectedObject.target = target.gameObject;
                detectedObject.distance = Vector2.Distance(transform.position, target.transform.position);
                detectedObject.velocity = target.GetComponent<Rigidbody2D>().velocity.magnitude;
                detectedObject.direction = directionToTarget;
                detectedObjects.Add(detectedObject);
            }
        }

        if (detectedObjects.Count > 0 && !playerDetected)
        {
            playerDetected = true;
            OnPlayerDetected?.Invoke(detectedObjects[0].target);
        }
        else if (detectedObjects.Count == 0)
        {
            playerDetected = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        foreach (LayerMask layer in targetLayers)
        {
            if (((1 << collider.gameObject.layer) & layer) != 0)
            {
                targetsInViewRadius.Add(collider);
                break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        foreach (LayerMask layer in targetLayers)
        {
            if (((1 << collider.gameObject.layer) & layer) != 0)
            {
                targetsInViewRadius.Remove(collider);
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!isGizmo)
        {
            return;
        }

        if (Application.isPlaying)
        {
            //do stuff

            Gizmos.color = detectedObjects.Count > 0 ? Color.red : gizmoColor;

            // Calculate the center of the FOV arc
            Vector3 center = transform.position;

            // Calculate the start and end angles of the FOV arc
            float startAngle = Mathf.Atan2(fovDirection.y, fovDirection.x) * Mathf.Rad2Deg - viewAngle / 2f;
            float endAngle = startAngle + viewAngle;

            // Draw a wireframe sphere to help visualize the arc
            Gizmos.DrawWireSphere(center, viewRadius);

            // Draw the arc using rays
            int numSegments = 2; // Reduce the number of segments
            float segmentAngle = (endAngle - startAngle) / numSegments;
            for (int i = 0; i <= numSegments; i++)
            {
                float a = startAngle + i * segmentAngle;
                Vector3 direction = new Vector3(Mathf.Cos(a * Mathf.Deg2Rad), Mathf.Sin(a * Mathf.Deg2Rad), 0);
                Gizmos.DrawRay(center, direction * viewRadius);
            }
        }
    }

    private Vector2 GetFOVDirection(FOVDirection_ direction)
    {
        Vector2 fovDirection = Vector2.zero;

        switch (direction)
        {
            case FOVDirection_.Forward:
                fovDirection = transform.up; // pointing upwards
                break;
            case FOVDirection_.Backward:
                fovDirection = -transform.up; // pointing downwards
                break;
            case FOVDirection_.Left:
                fovDirection = -transform.right; // pointing to the left
                break;
            case FOVDirection_.Right:
                fovDirection = transform.right; // pointing to the right
                break;
            case FOVDirection_.TopLeft:
                fovDirection = (transform.up + -transform.right).normalized; // pointing upwards and to the left
                break;
            case FOVDirection_.TopRight:
                fovDirection = (transform.up + transform.right).normalized; // pointing upwards and to the right
                break;
            case FOVDirection_.BottomLeft:
                fovDirection = (-transform.up + -transform.right).normalized; // pointing downwards and to the left
                break;
            case FOVDirection_.BottomRight:
                fovDirection = (-transform.up + transform.right).normalized; // pointing downwards and to the right
                break;
        }

        return fovDirection;
    }
}