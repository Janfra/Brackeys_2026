using System;
using UnityEngine;

[Serializable]
public struct SpeedStats
{
    [Min(0.0f)]
    public float MaxSpeed;

    [Min(0.0f)]
    public float AccelerationRate;

    [Min(0.0f)]
    public float DecelerationRate;

    public SpeedStats(float maxSpeed = 12.0f, float accelerationRate = 10.0f, float decelerationRate = 11.0f)
    {
        MaxSpeed = maxSpeed;
        AccelerationRate = accelerationRate;
        DecelerationRate = decelerationRate;
    }
}
