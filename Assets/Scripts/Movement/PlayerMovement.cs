using Janito.EditorExtras;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    [CreateButton(namingFormat: "{name} Movement Configuration", savePath: "Assets/ScriptableObjects/Configurations/Movement")]
    [InlineInspector]
    private MovementConfiguration defaultConfiguration;

    private float throttle;
    private float steer;
    private MovementConfiguration configuration;

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

    private Rigidbody rb;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        configuration = defaultConfiguration;
    }

    private void FixedUpdate()
    {
        UpdateSpeed();
        SetFacingDirection();
        MovePlayer();
    }

    public void OnThrottle(InputAction.CallbackContext context)
    {
        throttle = context.ReadValue<float>();
        if (throttle != 0)
        {
            validInputHorizontalDirection.x = throttle;
        }
    }


    public void OnSteer(InputAction.CallbackContext context)
    {
        steer = context.ReadValue<float>();
        if (steer != 0)
        {
            validInputHorizontalDirection.y = steer;
        }
    }

    private void UpdateSpeed()
    {
        if (throttle != 0)
        {
            // Could replace this with a curve to have finer control, but for now it works
            speed = Mathf.MoveTowards(speed, configuration.MaxSpeed, configuration.AccelerationRate * Time.deltaTime);
        }
        else // Not handling reversing yet
        {
            speed = Mathf.MoveTowards(speed, 0.0f, configuration.DecelerationRate * Time.deltaTime);
        }
    }

    private void MovePlayer()
    {
        if (throttle != 0)
        {
            velocity = transform.forward * throttle * speed;
        }
        else
        {
            velocity = transform.forward * validInputHorizontalDirection.x * speed;
        }
            
        rb.linearVelocity = velocity;
    }

    private void SetFacingDirection()
    {
        if (steer == 0.0f)
        {
            return;
        }

        var rotation = transform.eulerAngles;
        var targetRotation = rotation.y + (steer * configuration.RotationStep);
        var delta = configuration.RotationSpeed * Time.deltaTime;
        var newRotation = Mathf.MoveTowardsAngle(rotation.y, targetRotation, delta);

        rb.MoveRotation(Quaternion.Euler(rotation.x, newRotation, rotation.z));
    }
}
