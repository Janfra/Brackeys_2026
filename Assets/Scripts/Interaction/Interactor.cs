using Janito.EditorExtras;
using UnityEngine;
using UnityEngine.Events;

public class Interactor : MonoBehaviour
{
    public event UnityAction<IInteractable> OnInteracted;
    public event UnityAction OnTryInteract;

    [SerializeField]
    private float interactRadius;

    [SerializeField]
    private LayerMask interactLayerMask;

    [SerializeField]
    [ReadOnly]
    private InteractPayload interactPayload;

    [ReadOnly]
    public bool CanInteract;

    private void Awake()
    {
        interactPayload.Source = gameObject;
    }

    [Button(ButtonExecutionModes.PlayMode)]
    public void TryInteract()
    {
        OnTryInteract?.Invoke();
        if (!CanInteract)
        {
            return;
        }

        var nearbyItems = Physics.OverlapSphere(transform.position, interactRadius, interactLayerMask.value);
        foreach (var item in nearbyItems)
        {
            if (item.TryGetComponent(out IInteractable interactable) && interactable.IsInteractable)
            {
                InteractWith(interactable);
                break;
            }
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
