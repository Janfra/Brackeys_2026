using Janito.EditorExtras;
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Deliverable : IDisposable
{
    public event UnityAction<DeliveryResult> OnDelivered
    {
        add
        {
            onDelivered.AddListener(value);
        }
        remove
        {
            onDelivered.RemoveListener(value);
        }
    }

    [Header("Events")]
    [SerializeField]
    private UnityEvent<DeliveryResult> onDelivered;

    [Header("Debug")]
    [ReadOnly]
    public DeliveryDetailsSO DeliveryDetails;

    public bool IsDeliveryActive { get; private set; }

    private IDeliveryManager deliveryRegistry;

    public void Initialize(IDeliveryManager registry)
    {
        deliveryRegistry = registry;
        if (deliveryRegistry == null)
        {
            LogLibrary.LogErrorInDevelopment<Deliverable>($"Delivery registry is null. Registry must be provided to generate package details.");
        }
    }

    public void Deliver(DeliveryResult result)
    {
        onDelivered?.Invoke(result);
        deliveryRegistry.RemoveDeliveryOrder(DeliveryDetails, result);
        ClearDeliveryDetails();
    }

    public void Update(float deltaTime)
    {
        if (DeliveryDetails)
        {
            DeliveryDetails.ExpirationTimer.Update(deltaTime);
        }
    }

    public void OnEnable()
    {
        GetNewDeliveryDetails();
    }

    public void OnDisable()
    {
        ClearDeliveryDetails();
    }

    public void Dispose()
    {
        ClearDeliveryDetails();
        onDelivered.RemoveAllListeners();
    }

    private void GetNewDeliveryDetails()
    {
        DeliveryDetails = deliveryRegistry.GetNewDeliveryOrder();
        DeliveryDetails.ExpirationTimer.OnCompleted += FailDelivery;
        DeliveryDetails.ExpirationTimer.IsRunning = true;
        IsDeliveryActive = true;
    }

    private void ClearDeliveryDetails()
    {
        if (DeliveryDetails)
        {
            DeliveryDetails.ExpirationTimer.OnCompleted -= FailDelivery; // Should not need to clean up since dispose clears it
            deliveryRegistry.RemoveDeliveryOrder(DeliveryDetails, DeliveryResult.Failure); // Assume it was a failure if it was not delivered before being disabled
            DeliveryDetails = null;
            IsDeliveryActive = false;
        }
    }

    private void FailDelivery()
    {
        Deliver(DeliveryResult.Failure);
    }
}
