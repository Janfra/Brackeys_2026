using Janito.EditorExtras;
using System;
using UnityEngine;

[Serializable]
public class SteerMovement
{
    public Rigidbody Rigidbody;
    public Transform Transform;

    [Header("Debug")]
    [ReadOnly]
    [SerializeField]
    private float speed;
    [ReadOnly]
    [SerializeField]
    private Vector3 velocity;
    [ReadOnly]
    [SerializeField]
    private Vector2 validInputHorizontalDirection;
    
    private MovementConfigurationSO configuration;
    private float throttle;
    private float steer;

    public void SetConfiguration(MovementConfigurationSO config)
    {
        configuration = config;
    }

    // Can add move towards method in case of reusing for AI or similar which only a position is provided instead
    public void Move(Vector2 input, float deltaTime)
    {
        UpdateInput(input);
        UpdateSpeed(deltaTime);
        SetFacingDirection(deltaTime);
        UpdateVelocity();
    }

    private void UpdateInput(Vector2 input)
    {
        input.Normalize();
        throttle = input.x;
        if (throttle != 0)
        {
            validInputHorizontalDirection.x = throttle;
        }

        steer = input.y;
        if (steer != 0)
        {
            validInputHorizontalDirection.y = steer;
        }
    }

    private void UpdateSpeed(float deltaTime)
    {
        if (throttle != 0)
        {
            // Could replace this with a curve to have finer control, but for now it works
            speed = Mathf.MoveTowards(speed, configuration.MaxSpeed, configuration.AccelerationRate * deltaTime);
        }
        else // Not handling reversing yet
        {
            speed = Mathf.MoveTowards(speed, 0.0f, configuration.DecelerationRate * deltaTime);
        }
    }

    private void UpdateVelocity()
    {
        if (throttle != 0)
        {
            velocity = Transform.forward * throttle * speed;
        }
        else
        {
            velocity = Transform.forward * validInputHorizontalDirection.x * speed;
        }

        Rigidbody.linearVelocity = velocity;
    }

    private void SetFacingDirection(float deltaTime)
    {
        if (steer == 0.0f)
        {
            return;
        }

        var rotation = Transform.eulerAngles;
        var targetRotation = rotation.y + (steer * configuration.RotationStep);
        var delta = configuration.RotationSpeed * deltaTime;
        var newRotation = Mathf.MoveTowardsAngle(rotation.y, targetRotation, delta);

        Rigidbody.MoveRotation(Quaternion.Euler(rotation.x, newRotation, rotation.z));
    }
}
