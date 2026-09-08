using UnityEngine.Events;

public class OnlyAllowInteractOfType<T> : IInteractBlocker
    where T : IInteractable
{
    public UnityAction<bool> OnDeterminedOutcome;

    private IInteractor interactor;

    public bool CanContinueInteraction()
    {
        bool canContinue = false;
        if (interactor.TryGetTarget(out var interactable))
        {
            canContinue = interactable is T;
        }

        OnDeterminedOutcome?.Invoke(canContinue);
        return canContinue;
    }

    public void OnAssigned(IInteractor interactor)
    {
        this.interactor = interactor;
    }

    public void OnRemoved()
    {
        interactor = null;
    }
}
