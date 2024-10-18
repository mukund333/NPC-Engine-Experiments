using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVVisualizer
{
    private FOVConfiguration config;

    public FOVVisualizer(FOVConfiguration config)
    {
        this.config = config;
    }

    public void DrawGizmos(bool isPlayerDetected)
    {
        Gizmos.color = isPlayerDetected ? Color.red : Color.green;

        Vector2 fovDirection = DirectionUtility.GetFOVDirectionFromTransform(config.OriginTransform, config.FovDirectionType);
        float startAngle = Mathf.Atan2(fovDirection.y, fovDirection.x) * Mathf.Rad2Deg - config.ViewAngle / 2f;
        float endAngle = startAngle + config.ViewAngle;

        Gizmos.DrawWireSphere(config.Origin, config.ViewRadius);

        int numSegments = 2;
        float segmentAngle = (endAngle - startAngle) / numSegments;
        for (int i = 0; i <= numSegments; i++)
        {
            float angle = startAngle + i * segmentAngle;
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
            Gizmos.DrawRay(config.Origin, direction * config.ViewRadius);
        }
    }
}