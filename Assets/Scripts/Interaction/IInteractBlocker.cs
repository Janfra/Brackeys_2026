public interface IInteractBlocker
{
    public void OnAssigned(IInteractor interactor);
    public void OnRemoved();

    /// <summary>
    /// Returns whether the interaction can be completed.
    /// </summary>
    /// <returns></returns>
    public bool CanContinueInteraction();

    /// <summary>
    /// Returns the outcome of <c>CanContinueInteraction</c> outside of an actual interaction.
    /// </summary>
    /// <returns>Result of <c>CanContinueInteraction</c></returns>
    public bool WillAllowInteraction();
}
