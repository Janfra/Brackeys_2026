using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct FloatRange
{
    [Min(0.0f)]
    public float Min;

    [Min(0.0f)]
    public float Max;

    public FloatRange(float min = 0.0f, float max = 1.0f)
    {
        Min = Mathf.Min(min, max);
        Max = Mathf.Max(max, min);
    }

    public float GetRandomInRange()
    {
        return Random.Range(Min, Max);
    }
}
