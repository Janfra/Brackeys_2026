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
        animator.SetFloat(runMultiplierParameter, runMultiplierCurve.Evaluate(movementData.Speed / movementData.MaxSpeed));
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            interactor.TryInteract();
        }
    }
}
