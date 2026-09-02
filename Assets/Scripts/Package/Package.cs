using Janito.EditorExtras;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Package : MonoBehaviour, IInteractable, ISpawnable, IGrabbrableSource, IDeliveryDetailsHolder
{
    public event UnityAction<DeliveryResult> OnDelivered
    {
        add
        {
            deliverable.OnDelivered += value;
        }
        remove
        {
            deliverable.OnDelivered -= value;
        }
    }

    [SerializeField]
    [CreateButton(namingFormat: "{name} Grab Configuration", savePath: PathUtils.ProjectConfigurationPath + "/Grab")]
    [InlineInspector]
    private GrabConfigurationSO grabConfiguration;

    [SerializeField]
    [InlineInspector]
    private DeliveryRegistrySO deliveryRegistry;

    [SerializeField]
    private Deliverable deliverable;

    public DeliveryDetailsSO DeliveryDetails => deliverable.DeliveryDetails;
    public bool IsDeliveryActive => deliverable.IsDeliveryActive;
    public bool IsInteractable => interactor == null;
    public ISpawnableDespawner Despawner { get; set; }

    private Grabbable grabbable;
    private Interactor interactor;

    public Rigidbody Rigidbody { get; private set; }
    public Transform Transform => transform;
    public GameObject GrabObject => gameObject;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        grabbable = new(this, grabConfiguration);
        deliverable.Initialize(deliveryRegistry);
    }

    private void OnEnable()
    {
        deliverable.OnEnable();
    }

    private void OnDisable()
    {
        deliverable.OnDisable();
    }

    private void Update()
    {
        deliverable.Update(Time.deltaTime);
    }

    public void Deliver(DeliveryResult result)
    {
        Release();
        deliverable.Deliver(result);
    }

    public void Despawn()
    {
        // Clear remaining velocities for clean up
        Rigidbody.linearVelocity = Vector3.zero;
        Rigidbody.angularVelocity = Vector3.zero;
        Despawner.Despawn(gameObject);
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
