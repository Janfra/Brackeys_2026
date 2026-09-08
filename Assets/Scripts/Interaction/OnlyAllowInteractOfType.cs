using UnityEngine.Events;

public class OnlyAllowInteractOfType<T> : IInteractBlocker
    where T : IInteractable
{
    public UnityAction<bool> OnDeterminedOutcome;

    private IInteractor interactor;

    public bool WillAllowInteraction()
    {
        return interactor.TryGetTarget(out var interactable) && interactable is T;
    }

    public bool CanContinueInteraction()
    {
        bool canContinue = WillAllowInteraction();
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
