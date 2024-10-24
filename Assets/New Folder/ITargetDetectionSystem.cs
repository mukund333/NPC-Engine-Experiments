// IDetectionSystem.cs
using System.Collections.Generic;
using UnityEngine;

public interface ITargetDetectionSystem
{
    List<Target_Struct> GetDetectedTargets();
}