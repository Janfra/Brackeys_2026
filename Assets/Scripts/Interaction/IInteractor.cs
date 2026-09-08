using UnityEngine.Events;

public interface IInteractor
{
    public event UnityAction<IInteractable> OnInteracted;
    public bool CanInteract { get; set; }
    public IInteractSelector InteractSelector { get; set; }
    public IInteractBlocker InteractBlocker { get; }

    public bool TryGetTarget(out IInteractable interactable);
    public void TryInteract();
    public void SetBlocker(IInteractBlocker blocker);
}
