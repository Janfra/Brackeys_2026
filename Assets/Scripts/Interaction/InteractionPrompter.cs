using System;
using UnityEngine;

[Serializable]
public class InteractionPrompter
{
    public PromptSO Prompt;
    public IInteractor Interactor;

    public void UpdatePrompt()
    {
        if (Prompt != null && Interactor != null && Prompt.TryGetPrompter(out IPrompter prompter))
        {
            bool canInteract = Interactor.InteractBlocker != null ? Interactor.InteractBlocker.WillAllowInteraction() : true;
            if (Interactor.TryGetTarget(out IInteractable target) && canInteract)
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
}
