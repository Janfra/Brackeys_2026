using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Interactor), typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private MovementAnimator runAnimator;

    private Interactor interactor;
    private PlayerMovement movement;

    private void Awake()
    {
        interactor = GetComponent<Interactor>();
        movement = GetComponent<PlayerMovement>();
        runAnimator.Initialize(movement.ReadOnlyMovement);
    }

    private void LateUpdate()
    {
        runAnimator.UpdateMoveAnimation(isPlayedBackwards: movement.LastValidInput.x < 0);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            interactor.TryInteract();
        }
    }
}
