using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 90f;
    float throttle;
    float steer;

    Vector2 movementDirection = Vector2.zero;
    private Rigidbody rb;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }


    public void OnThrottle(InputAction.CallbackContext context)
    {
        throttle = context.ReadValue<float>();
        print("throttle: " + throttle);
    }


    public void OnSteer(InputAction.CallbackContext context)
    {
        steer = context.ReadValue<float>();
        print("steer: " + steer);
    }

    private void MovePlayer()
    {
        rb.linearVelocity = transform.forward * throttle * speed + transform.right * steer * speed;
    }
} 
