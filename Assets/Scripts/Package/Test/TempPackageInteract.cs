using Janito.EditorExtras;
using UnityEngine;

public class TempPackageInteract : MonoBehaviour, IPackageInteractable
{
    public bool IsInteractable => true;

    public void Interact(InteractPayload payload)
    {
        var package = payload.Source.GetComponentInChildren<Package>();
        if (package != null)
        {
            this.LogInDevelopment("Packaged");
            package.Grab(transform);
        }
    }
}
