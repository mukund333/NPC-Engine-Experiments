using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    private Vector2 fovDirection = Vector2.up;

    [SerializeField] Transform target;

    [Range(1, 180)]
    public float viewAngle;

    //Gizmos params
    public bool isGizmo;
    public float viewRadius;

    private void Start()
    {
        viewAngle = 90f;
        viewRadius = 3f;//get from radial
    }

    private float GetDotProduct(Transform target)
    {
        if (target == null)
        {
            Debug.LogError("No target  set for FieldOfView detection!");
        }

        Vector2 directionToTarget = (target.transform.position - transform.position).normalized;
        float dotProduct = Vector2.Dot(directionToTarget, fovDirection);
        return dotProduct;
    }

    private bool IsTargetWithinViewAngle(float viewAngle, float dotProduct)
    {
        return dotProduct > Mathf.Cos(viewAngle * Mathf.Deg2Rad / 2f);
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
            Gizmos.color = Color.green;
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





}
