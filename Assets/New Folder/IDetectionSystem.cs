// IDetectionSystem.cs
using System.Collections.Generic;
using UnityEngine;

public interface IDetectionSystem
{
    List<Target> GetDetectedTargets();
}