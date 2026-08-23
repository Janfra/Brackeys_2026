using UnityEngine;

public interface IInteractable
{
    public bool IsInteractable { get; }
    public void Interact(InteractPayload payload);
}

public class InteractPayload
{
    public GameObject Source;
}
