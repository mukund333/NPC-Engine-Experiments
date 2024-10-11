using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectPool
{
    private static Queue<DetectedObject> pool = new Queue<DetectedObject>();

    public static DetectedObject Get(GameObject target, Vector2 origin, Vector2 targetPosition, Rigidbody2D targetRigidbody)
    {
        if (pool.Count == 0)
        {
            pool.Enqueue(new DetectedObject());
        }

        DetectedObject obj = pool.Dequeue();
        obj.Target = target;
        obj.Distance = Vector2.Distance(origin, targetPosition);
        obj.Velocity = targetRigidbody != null ? targetRigidbody.velocity.magnitude : 0;
        obj.Direction = (targetPosition - origin).normalized;

        return obj;
    }

    public static void Return(DetectedObject obj)
    {
        obj.Target = null;
        pool.Enqueue(obj);
    }
}