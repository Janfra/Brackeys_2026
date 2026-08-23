using System;
using UnityEngine;

public interface IInteractable
{
    public bool IsInteractable { get; }
    public void Interact(InteractPayload payload);
}

[Serializable]
public class InteractPayload
{
    public GameObject Source;
}
