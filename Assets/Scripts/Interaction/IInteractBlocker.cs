public interface IInteractBlocker
{
    public void OnAssigned(IInteractor interactor);
    public void OnRemoved();
    public bool CanContinueInteraction();
}
