using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class FieldOfView : MonoBehaviour
{
    public FOVConfiguration Configuration;
    private FOVCore fovCore;
    private FOVEventManager eventManager;
    private FOVVisualizer visualizer;
    private CircleCollider2D triggerCollider;
    private Coroutine updateCoroutine;

    [SerializeField] private float updateInterval = 0.1f;

    private void Start()
    {
        Configuration.Origin = transform.position;
        Configuration.OriginTransform = transform; // Add this line
        fovCore = new FOVCore(Configuration);

        eventManager = new FOVEventManager();
        visualizer = new FOVVisualizer(Configuration);

        triggerCollider = GetComponent<CircleCollider2D>();
        triggerCollider.radius = Configuration.ViewRadius;
        triggerCollider.isTrigger = true;

        updateCoroutine = StartCoroutine(UpdateFOVRoutine());
    }

    private IEnumerator UpdateFOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(updateInterval);
        while (true)
        {
            Configuration.Origin = transform.position;
            fovCore.UpdateConfiguration(Configuration);
            var detectedObjects = fovCore.GetDetectedObjects();
            eventManager.UpdateDetectionStatus(detectedObjects);

            foreach (var obj in detectedObjects)
            {
                ObjectPool.Return(obj);
            }

            yield return wait;
        }
    }

    private void OnDisable()
    {
        if (updateCoroutine != null)
        {
            StopCoroutine(updateCoroutine);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (IsInTargetLayers(collider.gameObject.layer))
        {
            fovCore.AddTarget(collider);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (IsInTargetLayers(collider.gameObject.layer))
        {
            fovCore.RemoveTarget(collider);
        }
    }

    private bool IsInTargetLayers(int layer)
    {
        return (Configuration.TargetLayers.value & (1 << layer)) != 0;
    }
    public DetectedObject GetMostThreateningObstacle()
    {
        return fovCore.GetMostThreateningObstacle();
    }
    private void OnDrawGizmos()
    {
        if (Application.isPlaying && visualizer != null)
        {
            visualizer.DrawGizmos(fovCore.GetDetectedObjects().Count > 0);
        }
    }
}