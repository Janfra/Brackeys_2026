using Janito.EditorExtras;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    [CreateButton(namingFormat: "{name} Movement Configuration", savePath: PathUtils.ProjectConfigurationPath + "/Movement")]
    [InlineInspector]
    private MovementConfigurationSO defaultConfiguration;

    [SerializeField]
    [ReadOnly]
    private SteerMovement steerMovement = new();
    private Vector2 input;

    public IReadOnlyMovement ReadOnlyMovement => steerMovement;
    public Vector2 Input => input;

    private void Awake()
    {
        steerMovement.Rigidbody = GetComponent<Rigidbody>();
        steerMovement.Transform = transform;
        steerMovement.SetConfiguration(defaultConfiguration);
    }

    private void FixedUpdate()
    {
        steerMovement.Move(input, Time.fixedDeltaTime);
    }

    public void OnThrottle(InputAction.CallbackContext context)
    {
        input.x = context.ReadValue<float>();
    }

    public void OnSteer(InputAction.CallbackContext context)
    {
        input.y = context.ReadValue<float>();
    }
}
