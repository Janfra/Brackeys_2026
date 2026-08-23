using Janito.EditorExtras;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Interactor : MonoBehaviour
{
    public event UnityAction<IInteractable> OnInteracted;

    [SerializeField]
    private float interactRadius;

    [SerializeField]
    private LayerMask interactLayerMask;

    [SerializeField]
    [ReadOnly]
    private InteractPayload interactPayload;

    [ReadOnly]
    public bool CanInteract;

    /// <summary>
    /// To be able to assign custom logic before on interact default logic. Returned boolean determines if we should continue logic.
    /// </summary>
    public Func<List<IInteractable>, bool> OnShouldInteract;

    private List<IInteractable> interactSweep = new();

    private void Awake()
    {
        interactPayload.Source = gameObject;
    }

    [Button(ButtonExecutionModes.PlayMode)]
    public void TryInteract()
    {
        if (!CanInteract)
        {
            return;
        }

        interactSweep.Clear();
        var nearbyItems = Physics.OverlapSphere(transform.position, interactRadius, interactLayerMask.value);
        var orderedByDistance = nearbyItems.OrderBy(x => (transform.position - x.transform.position).sqrMagnitude);
        foreach (var item in orderedByDistance)
        {
            if (item.TryGetComponent(out IInteractable interactable) && interactable.IsInteractable)
            {
                interactSweep.Add(interactable);
            }
        }

        if (OnShouldInteract != null)
        {
            if (!OnShouldInteract.Invoke(interactSweep))
            {
                return;
            }
        }

        if (interactSweep.Count > 0)
        {
            InteractWith(interactSweep[0]);
        }
    }

    private void InteractWith(IInteractable interactable)
    {
        interactable.Interact(interactPayload);
        OnInteracted?.Invoke(interactable);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
