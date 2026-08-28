using UnityEngine;

public interface IReadOnlyMovement
{
    public float Speed { get; }
    public float MaxSpeed { get; }
    public Vector3 Velocity { get; }
}
