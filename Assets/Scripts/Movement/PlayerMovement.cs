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


    void OnThrottle(InputValue value)
{
        throttle = value.Get<float>();
    print(throttle);
}


    void OnSteer(InputValue value)
{
        steer = value.Get<float>();
        print(steer);
}

private void MovePlayer()
    {
        rb.linearVelocity = transform.forward * throttle * speed + transform.right * steer * speed;
}
} 
