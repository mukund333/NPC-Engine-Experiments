using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FOVCore
{
    private FOVConfiguration config;
    private HashSet<Collider2D> targetsInViewRadius = new HashSet<Collider2D>();
    private List<DetectedObject> detectedObjects = new List<DetectedObject>();
    private float cosHalfViewAngle;
    private DetectedObject mostThreateningObstacle = null;
    public FOVCore(FOVConfiguration config)
    {
        this.config = config;
        UpdateCachedValues();
    }

    public void UpdateConfiguration(FOVConfiguration newConfig)
    {
        this.config = newConfig;
        UpdateCachedValues();
    }

    private void UpdateCachedValues()
    {
        cosHalfViewAngle = Mathf.Cos(config.ViewAngle * Mathf.Deg2Rad / 2f);
    }

    public List<DetectedObject> GetDetectedObjects()
    {
        detectedObjects.Clear();
        Vector2 fovDirection = DirectionUtility.GetFOVDirectionFromTransform(config.OriginTransform, config.FovDirectionType);
        float closestDistance = float.MaxValue; // To track the closest obstacle

        foreach (var target in targetsInViewRadius)
        {
            if (target == null) continue;

            Vector2 targetPosition = target.transform.position;
            Vector2 directionToTarget = (targetPosition - (Vector2)config.Origin).normalized;
            float dotProduct = Vector2.Dot(directionToTarget, fovDirection);

            if (dotProduct > cosHalfViewAngle)
            {
                DetectedObject detectedObj = ObjectPool.Get(target.gameObject, (Vector2)config.Origin, targetPosition, target.attachedRigidbody);
                detectedObjects.Add(detectedObj);

                // Calculate proximity (distance) to determine the most threatening obstacle
                float distance = Vector2.Distance(config.Origin, targetPosition);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    mostThreateningObstacle = detectedObj; // Update the most threatening obstacle
                }
            }
        }

        return detectedObjects;
    }

    // Method to get the most threatening obstacle based on proximity
    public DetectedObject GetMostThreateningObstacle()
    {
        return mostThreateningObstacle;
    }

    public void AddTarget(Collider2D target) => targetsInViewRadius.Add(target);
    public void RemoveTarget(Collider2D target) => targetsInViewRadius.Remove(target);
}