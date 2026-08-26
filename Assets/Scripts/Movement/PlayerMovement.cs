using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] 
    private float speed = 5f;
    [SerializeField] 
    private float rotationSpeed = 90f;
    [SerializeField, Range(1f, 179f)]
    private float rotationStep = 170.0f;

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
        velocity = transform.forward * inputHorizontalDirection.x * speed;
        rb.linearVelocity = velocity;
    }

    private void SetFacingDirection()
    {
        if (steer == 0.0f)
        {
            return;
        }

        var rotation = transform.eulerAngles;
        var targetRotation = rotation.y + (steer * rotationStep);
        var delta = rotationSpeed * Time.deltaTime;
        var newRotation = Mathf.MoveTowardsAngle(rotation.y, targetRotation, delta);

        rb.MoveRotation(Quaternion.Euler(rotation.x, newRotation, rotation.z));
    }

    private void OnValidate()
    {
        rotationSpeed = Mathf.Max(0, rotationSpeed);
        speed = Mathf.Max(0, speed);
    }
} 
