using System;
using UnityEngine;

[Serializable]
public struct DirectionalThrowForce
{
    [Min(0.0f)]
    public float ForwardStrength;

    [Min(0.0f)]
    public float UpwardStrength;

    public DirectionalThrowForce(float forwardStrength = 7.5f, float upwardStrength = 2.0f)
    {
        ForwardStrength = forwardStrength;
        UpwardStrength = upwardStrength;
    }
}
