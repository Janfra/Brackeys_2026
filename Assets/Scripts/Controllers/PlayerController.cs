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
    private PromptSO interactPrompt;

    private IInteractor interactor;
    private PlayerMovement movement;

    private void Awake()
    {
        interactor = GetComponent<IInteractor>();
        movement = GetComponent<PlayerMovement>();
        runAnimator.Initialize(movement.ReadOnlyMovement);
    }

    private void LateUpdate()
    {
        runAnimator.UpdateMoveAnimation(isPlayedBackwards: movement.LastValidInput.x < 0);

        if (interactPrompt && interactPrompt.TryGetPrompter(out IPrompter prompter))
        {
            bool canInteract = interactor.InteractBlocker != null ? interactor.InteractBlocker.WillAllowInteraction() : true;
            if (interactor.TryGetTarget(out IInteractable target) && canInteract)
            {
                if (target is IPromptTarget promptTarget)
                {
                    prompter.SetPrompt(promptTarget.GetPromptSettings());
                }
                else if (target is Component targetComponent && targetComponent.TryGetComponent(out promptTarget)) 
                {
                    prompter.SetPrompt(promptTarget.GetPromptSettings());
                }
                else
                {
                    prompter.HidePrompt();
                }
            }
            else
            {
                prompter.HidePrompt();
            }
        } 
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
