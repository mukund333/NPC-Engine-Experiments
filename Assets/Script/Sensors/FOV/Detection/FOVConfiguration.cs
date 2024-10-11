using UnityEngine;

[CreateAssetMenu(fileName = "NewFOVConfiguration", menuName = "FOV/Configuration", order = 1)]
public class FOVConfiguration : ScriptableObject
{
    [Range(1, 180)] public float ViewAngle = 45f;
    public float ViewRadius = 3f;
    public LayerMask TargetLayers;
    public FOVDirection FovDirectionType = FOVDirection.Forward;
    [HideInInspector] public Vector2 Origin;

    // New field to store the transform of the object with FOV
    [HideInInspector] public Transform OriginTransform;
}