using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FOVEventManager
{
    public event Action<GameObject> OnPlayerDetected;
    public event Action OnPlayerLost;

    private bool playerDetected = false;

    public void UpdateDetectionStatus(List<DetectedObject> detectedObjects)
    {
        if (detectedObjects.Count > 0 && !playerDetected)
        {
            playerDetected = true;
            OnPlayerDetected?.Invoke(detectedObjects[0].Target);
        }
        else if (detectedObjects.Count == 0 && playerDetected)
        {
            playerDetected = false;
            OnPlayerLost?.Invoke();
        }
    }
}

