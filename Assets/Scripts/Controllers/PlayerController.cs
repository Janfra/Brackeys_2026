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
        animator.SetFloat(velocityParameter, movementData.Speed);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            interactor.TryInteract();
        }
    }
}
