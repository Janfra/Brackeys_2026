using Janito.EditorExtras;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Interactor : MonoBehaviour, IInteractor
{
    public event UnityAction<IInteractable> OnInteracted;

    [SerializeField]
    private float interactRadius;

    [SerializeField]
    private LayerMask interactLayerMask;

    [SerializeField]
    private bool canInteractOnAwake;
    
    [SerializeField]
    [ReadOnly]
    private InteractPayload interactPayload;

    [field: ReadOnly]
    [field: SerializeField]
    public bool CanInteract { get; set; } = true;

    public IInteractSelector InteractSelector { get; set; }
    public IInteractBlocker InteractBlocker { get; private set; }
    
    private List<IInteractable> interactSweep = new();
    private IInteractable selectedInteractable => interactSweep[0];

    private void Awake()
    {
        interactPayload.Source = gameObject;
        CanInteract = canInteractOnAwake;
    }

    public bool TryGetTarget(out IInteractable interactable)
    {
        UpdateInteractList();

        interactable = null;
        if (interactSweep.Count > 0)
        {
            interactable = selectedInteractable;
        }

        return interactable != null;
    }

    public void UpdateInteractList()
    {
        interactSweep.Clear();
        var nearbyItems = Physics.OverlapSphere(transform.position, interactRadius, interactLayerMask.value);
        var orderedByDistance = nearbyItems.OrderBy(GetColliderDistance);
        foreach (var item in orderedByDistance)
        {
            if (item.TryGetComponent(out IInteractable interactable) && interactable.IsInteractable)
            {
                interactSweep.Add(interactable);
            }
        }

        InteractSelector?.SetSelectedInteract(interactSweep);
    }

    [Button(ButtonExecutionModes.PlayMode)]
    public void TryInteract()
    {
        if (!CanInteract)
        {
            return;
        }

        UpdateInteractList();
        
        if (InteractBlocker != null)
        {
            if (!InteractBlocker.CanContinueInteraction())
            {
                return;
            }
        }

        if (TryGetTarget(out IInteractable interactable))
        {
            InteractWith(interactable);
        }
    }

    public void SetBlocker(IInteractBlocker interactBlocker)
    {
        if (InteractBlocker == interactBlocker)
        {
            return;
        }

        if (InteractBlocker != null)
        {
            InteractBlocker.OnRemoved();
        }

        InteractBlocker = interactBlocker;
        if (InteractBlocker != null)
        {
            InteractBlocker.OnAssigned(this);
        }
    }

    private void InteractWith(IInteractable interactable)
    {
        interactable.Interact(interactPayload);
        OnInteracted?.Invoke(interactable);
    }

    private float GetColliderDistance(Collider collider)
    {
        return (transform.position - collider.transform.position).sqrMagnitude;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
