using UnityEngine;

public interface IInteractable
{
    public void Interact(InteractPayload payload);
}

public class InteractPayload
{
    public GameObject Source;
}
