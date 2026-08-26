using System;
using UnityEngine;

[Serializable]
public struct RotationStats
{
    public float RotationSpeed;

    [Range(1f, 179f)]
    public float RotationStep;

    public RotationStats(float rotationSpeed = 200.0f, float rotationStep = 170.0f)
    {
        RotationSpeed = rotationSpeed;
        RotationStep = rotationStep;
    }
}
