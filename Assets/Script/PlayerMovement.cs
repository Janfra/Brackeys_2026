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
}


    void OnSteer(InputValue value)
{
    steer = value.Get<float>();
}

private void MovePlayer()
    {
        rb.MovePosition(rb.position + transform.up * throttle * speed * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + transform.right * steer * speed * Time.fixedDeltaTime);
}
} 
