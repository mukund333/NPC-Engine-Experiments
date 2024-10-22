using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Entity : MonoBehaviour
{
    private InterestMapCalculator interestMap;
    private float[] currentInterestValues;

    [SerializeField] private float searchRadius;    // Radius of the circle
    [SerializeField] private float castDistance ;    // Distance to cast the circle
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float moveSpeed = 5f;

    // For debug visualization
    [SerializeField] private bool showDebug = true;
    [SerializeField] private float debugRayLength = 1f;

    private void Start()
    {
        interestMap = new InterestMapCalculator();
    }

    private void Update()
    {
        List<Target> activeTargets = new List<Target>();

        // Define the direction for the cast (e.g., forward or based on movement direction)
        Vector2 castDirection = transform.up;  // Change direction as needed

        // Use CircleCastAll instead of OverlapCircleAll
        RaycastHit2D[] hitObjects = Physics2D.CircleCastAll(transform.position, searchRadius, castDirection, castDistance, targetLayer);

        foreach (RaycastHit2D hit in hitObjects)
        {
            Collider2D obj = hit.collider;

            Target newTarget = new Target
            {
                position = obj.transform.position,
                weight = DetermineWeight(obj)
            };
            activeTargets.Add(newTarget);
        }

        currentInterestValues = interestMap.CalculateInterestMap(activeTargets, transform.position);
        Vector2 moveDirection = GetBestDirection();
        Move(moveDirection);
    }

    private float DetermineWeight(Collider2D targetObj)
    {
        if (targetObj.CompareTag("Dangers"))
            return 5f;
        if (targetObj.CompareTag("Elite"))
            return 3f;
        return 1f;
    }

    private Vector2 GetBestDirection()
    {
        float highestInterest = 0f;
        int bestIndex = 0;

        for (int i = 0; i < currentInterestValues.Length; i++)
        {
            if (currentInterestValues[i] > highestInterest)
            {
                highestInterest = currentInterestValues[i];
                bestIndex = i;
            }
        }

        return interestMap.GetDirections()[bestIndex];
    }

    private void Move(Vector2 direction)
    {
        //transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        if (!showDebug || currentInterestValues == null || interestMap == null) return;

        Vector2[] dirs = interestMap.GetDirections();

        for (int i = 0; i < currentInterestValues.Length; i++)
        {
            // Scale length by interest value
            float length = currentInterestValues[i] * debugRayLength;
            Vector3 direction = dirs[i] * length;

            // Color gradient from blue (low) to red (high)
            //Color rayColor = Color.Lerp(Color.blue, Color.red, currentInterestValues[i]);
            //Gizmos.color = rayColor;

            Gizmos.color = Color.green;    
            // Draw the direction ray
            Gizmos.DrawRay(transform.position, direction);

            // Draw sphere at end point for better visibility
            //Gizmos.DrawWireSphere(transform.position + direction, 0.1f);
        }

        //// Draw detection radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRadius);

       
    }
}
