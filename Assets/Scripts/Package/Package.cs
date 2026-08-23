using Janito.EditorExtras;
using UnityEngine;

public class Package : MonoBehaviour, IInteractable
{
    [SerializeField]
    private float heightOffset = 1.0f;

    public bool IsInteractable => true;

    public void Interact(InteractPayload payload)
    {
        if (payload == null || payload.Source == null) return;

        MoveOnTopOfInteractor(payload);   
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
