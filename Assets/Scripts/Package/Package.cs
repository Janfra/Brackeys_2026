using Janito.EditorExtras;
using System;
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

    private PackageInteractableSelector packageInteractableSelector = new();
    private OnlyAllowInteractOfType<IPackageInteractable> packageInteractionOnlyBlocker = new();
    private Grabbable grabbable;
    private IInteractor interactor;

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
        packageInteractionOnlyBlocker.OnDeterminedOutcome = HandleInteractOutcome;
    }

    private void OnDisable()
    {
        deliverable.OnDisable();
        packageInteractionOnlyBlocker.OnDeterminedOutcome = null;
        ClearInteractor();
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
        UpdateInteractConfiguration(payload);
    }

    public void Grab(Transform newHolder)
    {
        grabbable.Grab(newHolder);
    }

    public void Throw()
    {
        ClearInteractor();
        grabbable.Throw();
    }

    public void Release()
    {
        ClearInteractor();
        grabbable.Release();
    }

    private void UpdateInteractConfiguration(InteractPayload payload)
    {
        if (payload.Source.TryGetComponent(out interactor))
        {
            interactor.InteractSelector = packageInteractableSelector;
            interactor.SetBlocker(packageInteractionOnlyBlocker);
        }
    }

    private void HandleInteractOutcome(bool hasValidInteraction)
    {
        if (!hasValidInteraction)
        {
            Throw();
        }
    }

    private void ClearInteractor()
    {
        if (interactor != null)
        {
            interactor.InteractSelector = null;
            interactor.SetBlocker(null);
            interactor = null;
        }
    }
}
