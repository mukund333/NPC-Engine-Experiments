
using UnityEngine;

[RequireComponent(typeof(ObstacleDetector))]
public class Entity : MonoBehaviour
{
    private DangerMapCalculator dangerMap;
    private IDetectionSystem detectionSystem;
    private float[] currentDangerValues;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool showDebug = true;
    [SerializeField] private float debugRayLength = 1f;

    private void Start()
    {
        dangerMap = new DangerMapCalculator();
        detectionSystem = GetComponent<ObstacleDetector>();
    }

    private void Update()
    {
        var targets = detectionSystem.GetDetectedTargets();
        currentDangerValues = dangerMap.CalculateInterestMap(targets, transform.position);
        Vector2 moveDirection = GetBestDirection();
        //Move(moveDirection);
    }

    private Vector2 GetBestDirection()
    {
        float highestInterest = 0f;
        int bestIndex = 0;

        for (int i = 0; i < currentDangerValues.Length; i++)
        {
            if (currentDangerValues[i] > highestInterest)
            {
                highestInterest = currentDangerValues[i];
                bestIndex = i;
            }
        }

        return dangerMap.GetDirections()[bestIndex];
    }

    private void Move(Vector2 direction)
    {
        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        if (!showDebug || currentDangerValues == null || dangerMap == null) return;

        Vector2[] dirs = dangerMap.GetDirections();
        for (int i = 0; i < currentDangerValues.Length; i++)
        {
            float length = currentDangerValues[i] * debugRayLength;
            Vector3 direction = dirs[i] * length;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, direction);
        }
    }
}