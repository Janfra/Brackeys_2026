using UnityEngine;
using UnityEngine.InputSystem;
public class TruckMovement : MonoBehaviour
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
}


    void OnSteer(InputValue value)
{
    steer = value.Get<float>();
    print(steer);
}

private void MovePlayer()
{
    rb.linearVelocity = transform.up * throttle * speed;

    float rotationAmount = -steer * rotationSpeed * Time.fixedDeltaTime;
    Quaternion deltaRotation = Quaternion.Euler(0f, 0f, rotationAmount);
    rb.MoveRotation(rb.rotation * deltaRotation);
}
} 
