using Janito.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Interactor), typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private AnimatorParameterHasher velocityParameter;
    [SerializeField]
    private AnimatorParameterHasher runMultiplierParameter;
    [SerializeField]
    private AnimationCurve runMultiplierCurve;
    [SerializeField]
    private float minVelocity = 0.2f;

    private Interactor interactor;
    private PlayerMovement movement;
    private IReadOnlyMovement movementData;

    private void Awake()
    {
        interactor = GetComponent<Interactor>();
        movement = GetComponent<PlayerMovement>();
        movementData = movement.ReadOnlyMovement;
    }

    private void LateUpdate()
    {
        animator.SetFloat(velocityParameter, movementData.Velocity.sqrMagnitude > minVelocity ? movementData.Speed : 0.0f);
        var newRunMultiplier = runMultiplierCurve.Evaluate(movementData.Speed / movementData.MaxSpeed);
        animator.SetFloat(runMultiplierParameter, movement.Input.x > 0 ? newRunMultiplier : -newRunMultiplier); // Make multiplier negative if input is moving backwards to play inverse animation to show as running back
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            interactor.TryInteract();
        }
    }
}
