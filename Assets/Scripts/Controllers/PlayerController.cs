using Janito.EditorExtras;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Interactor), typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour, IGrabTracker
{
    [SerializeField]
    [CreateButton("{name} Grab Tracker", savePath: PathUtils.ProjectScriptableObjectsPath)]
    [InlineInspector]
    private GrabTrackerSO playerGrabTracker;

    [SerializeField]
    private MovementAnimator runAnimator;

    [SerializeField]
    private InteractionPrompter interactionPrompter;

    private IInteractor interactor;
    private PlayerMovement movement;

    private void Awake()
    {
        interactor = GetComponent<IInteractor>();
        movement = GetComponent<PlayerMovement>();
        runAnimator.Initialize(movement.ReadOnlyMovement);
        interactionPrompter.Interactor = interactor;
    }

    private void LateUpdate()
    {
        runAnimator.UpdateMoveAnimation(isPlayedBackwards: movement.LastValidInput.x < 0);
        interactionPrompter.UpdatePrompt();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            interactor.TryInteract();
        }
    }

    public void NotifyNewGrabbed(GrabInformation grabData)
    {
        playerGrabTracker.NotifyNewGrabbed(grabData);
    }
}
