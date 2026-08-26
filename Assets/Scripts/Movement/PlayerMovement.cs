using Janito.EditorExtras;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    [CreateButton(namingFormat: "{name} Movement Configuration", savePath: "Assets/ScriptableObjects/Configurations/Movement")]
    [InlineInspector]
    private MovementConfiguration configuration;

    private float throttle;
    private float steer;

    private Vector3 velocity = Vector3.zero;
    private Vector2 inputHorizontalDirection = Vector2.zero;

    private Rigidbody rb;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        UpdateInput();
        SetFacingDirection();
        MovePlayer();
    }

    public void OnThrottle(InputAction.CallbackContext context)
    {
        throttle = context.ReadValue<float>();
    }


    public void OnSteer(InputAction.CallbackContext context)
    {
        steer = context.ReadValue<float>();
    }

    private void UpdateInput()
    {
        inputHorizontalDirection = new Vector2(throttle, steer);
        inputHorizontalDirection.Normalize();
    }

    private void MovePlayer()
    {
        velocity = transform.forward * inputHorizontalDirection.x * configuration.Speed;
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
