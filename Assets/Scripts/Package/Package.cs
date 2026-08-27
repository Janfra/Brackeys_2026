using Janito.EditorExtras;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Package : MonoBehaviour, IInteractable
{
    [SerializeField]
    [CreateButton(namingFormat: "{name} Grab Configuration", savePath: PathUtils.ProjectConfigurationPath + "/Grab")]
    [InlineInspector]
    private GrabConfigurationSO grabConfiguration;

    public PackageDetailsSO deliveryDetails;

    public bool IsInteractable => interactor == null;

    private Grabbable grabbable = new();
    private Interactor interactor;

    private void Awake()
    {
        grabbable.Rigidbody = GetComponent<Rigidbody>();
        grabbable.Transform = transform;
        grabbable.GrabConfiguration = grabConfiguration;
    }

    public void Interact(InteractPayload payload)
    {
        if (payload == null || payload.Source == null || !IsInteractable) return;

        grabbable.Grab(payload.Source.transform);
        TrySetInteractorPackageOverride(payload);
    }

    public void Grab(Transform newHolder)
    {
        grabbable.Grab(newHolder);
    }

    public void Throw()
    {
        grabbable.Throw();
    }

    public void Release()
    {
        if (interactor != null)
        {
            interactor.OnShouldInteract = null;
            interactor = null;            
        }

        grabbable.Release();
    }

    private void TrySetInteractorPackageOverride(InteractPayload payload)
    {
        if (payload.Source.TryGetComponent(out interactor))
        {
            interactor.OnShouldInteract = HandleInteraction;
        }
    }

    private bool HandleInteraction(List<IInteractable> interactablesInRange)
    {
        interactor.OnShouldInteract = null;
        bool hasValidInteractable = HasPackageCompatibleInteractable(interactablesInRange); // Assigns it to be interacted with if found

        if (!hasValidInteractable)
        {
            grabbable.Throw();
        }

        interactor = null;
        return hasValidInteractable;
    }

    private bool HasPackageCompatibleInteractable(List<IInteractable> interactablesInRange)
    {
        for (int i = 0; i < interactablesInRange.Count; i++)
        {
            var interactable = interactablesInRange[i];
            if (interactable is IPackageInteractable)
            {
                if (i == 0) return true;

                // Swap it to first position and let it be interacted with
                var temp = interactablesInRange[0];
                interactablesInRange[i] = temp;
                interactablesInRange[0] = interactable;
                return true;
            }
        } 

        return false;
    }
}
