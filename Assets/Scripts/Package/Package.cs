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

        DisablePhysics();
        MoveOnTopOfObject(payload.Source.transform);
        TrySetInteractorPackageOverride(payload);
    }

    public void Collect()
    {
        if (interactor != null)
        {
            interactor.OnShouldInteract = null;
            interactor = null;            
        }
        transform.SetParent(null);
    }

    public void MoveOnTopOfObject(Transform target)
    {
        var position = target.position;
        position.y += heightOffset;

        if (TryGetHeightFromCollider(target.gameObject, out float yOffset))
        {
            position.y += yOffset;
        }
        else
        {
            this.LogWarningInDevelopment($"Unable to determine size of source from collider on {target.gameObject.name}. Using arbitrary offset instead.");
            position.y += heightOffset; // Do height offset again as a fallback for now to try to put it on top
        }

        transform.position = position;
        transform.SetParent(target.transform);
    }

    public void DisablePhysics()
    {
        rigidbody.isKinematic = true;
    }
    
    public void EnablePhysics()
    {
        rigidbody.isKinematic = false;
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
            ThrowPackage();
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

    private void ThrowPackage()
    {
        EnablePhysics();
        transform.SetParent(null);
        rigidbody.AddForce(interactor.transform.forward * throwForce, ForceMode.Impulse);
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
