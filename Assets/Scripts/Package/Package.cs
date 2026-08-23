using Janito.EditorExtras;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Package : MonoBehaviour, IInteractable
{
    [SerializeField]
    private float heightOffset = 1.0f;

    [SerializeField]
    private float throwForce = 50.0f; // Temporary throw force

    public bool IsInteractable => interactor == null;

    private Interactor interactor;
    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Interact(InteractPayload payload)
    {
        if (payload == null || payload.Source == null || !IsInteractable) return;

        MoveOnTopOfInteractor(payload);
        TrySetInteractorPackageOverride(payload);
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
        bool hasValidInteractable = HasPackageCompatibleInteractable(interactablesInRange);

        if (hasValidInteractable)
        {
            // Set them to be the selected interactable
        }
        else
        {
            ThrowPackage();
        }

        interactor = null;
        return hasValidInteractable;
    }

    private bool HasPackageCompatibleInteractable(List<IInteractable> interactablesInRange)
    {
        // Find interface in list or similar here
        return false;
    }

    private void ThrowPackage()
    {

        transform.SetParent(null);
        rigidbody.AddForce(interactor.transform.forward * throwForce, ForceMode.Impulse);
        interactor = null;
    }

    private void MoveOnTopOfInteractor(InteractPayload payload)
    {
        var position = payload.Source.transform.position;
        position.y += heightOffset;
        
        if (TryGetHeightFromCollider(payload.Source, out float yOffset))
        {
            position.y += yOffset;
        }
        else
        {
            this.LogWarningInDevelopment($"Unable to determine size of source from collider on {payload.Source.name}. Using arbitrary offset instead.");
            position.y += heightOffset; // Do height offset again as a fallback for now to try to put it on top
        }

        transform.position = position;
        transform.SetParent(payload.Source.transform, true);
    }

    private bool TryGetHeightFromCollider(GameObject go, out float height)
    {
        if (go.TryGetComponent(out Collider collider))
        {
            height = collider.bounds.extents.y;
            return true;
        }

        height = 0.0f;
        return false;
    }
}
