using Janito.EditorExtras;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryNoteController : MonoBehaviour
{
    private struct DeliveryNoteState
    {
        public bool IsOpened;

        public DeliveryNoteState(bool isOpened = true)
        {
            IsOpened = isOpened;
        }
    }

    [SerializeField]
    private DeliveryNoteDisplay deliveryNoteDisplay;
    [SerializeField]
    private DeliveryNoteButton deliveryNoteButton;
    [SerializeField]
    private bool isShownOnStart;

    [SerializeField]
    private GrabTrackerSO playerGrabTracker;

    private Dictionary<DeliveryDetailsSO, DeliveryNoteState> deliveryNoteStates = new();
    private DeliveryDetailsSO currentDetails;
    private GrabInformation grabInformation;

    private void Awake()
    {
        if (deliveryNoteDisplay == null)
        {
            this.LogErrorInDevelopment("DeliveryNoteDisplay is not assigned in the inspector.");
        }

        if (playerGrabTracker == null)
        {
            this.LogErrorInDevelopment("PlayerGrabTracker is not assigned in the inspector.");
        }

        if (!isShownOnStart)
        {
            deliveryNoteButton.gameObject.SetActive(false);
            deliveryNoteDisplay.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        playerGrabTracker.OnNewGrabbed += UpdateDeliveryNoteState;
        deliveryNoteDisplay.OnClosed = ShowButton;
        deliveryNoteButton.OnClicked = ShowDisplay;
    }

    private void OnDisable()
    {
        playerGrabTracker.OnNewGrabbed -= UpdateDeliveryNoteState;
        deliveryNoteDisplay.OnClosed = null;
        deliveryNoteButton.OnClicked = null;
    }

    private void UpdateDeliveryNoteState(GrabInformation grabInformation)
    {
        if (grabInformation.IsValid && grabInformation.GrabbedObject.TryGetComponent(out IDeliveryDetailsHolder deliveryDetailsHolder))
        {
            this.grabInformation = grabInformation;
            currentDetails = deliveryDetailsHolder.DeliveryDetails;
            TryEnableComponents();
            var isOpened = GetOpenState(currentDetails);
            deliveryNoteDisplay.SetDescription(deliveryDetailsHolder.DeliveryDetails.Description);
            if (isOpened)
            {
                deliveryNoteDisplay.Show();
            }
            else
            {
                deliveryNoteButton.Show();
            }

            grabInformation.ReleaseNotifier.OnReleased += HideAll;
        }
    }

    private void TryEnableComponents()
    {
        if (deliveryNoteDisplay.gameObject.activeSelf == false)
        {
            deliveryNoteDisplay.gameObject.SetActive(true);
        }

        if (deliveryNoteButton.gameObject.activeSelf == false)
        {
            deliveryNoteButton.gameObject.SetActive(true);
        }
    }

    private void HideAll()
    {
        grabInformation.ReleaseNotifier.OnReleased -= HideAll;
        grabInformation = null;
        deliveryNoteDisplay.Hide();
        deliveryNoteButton.Hide();
    }

    private void ShowDisplay()
    {
        deliveryNoteDisplay.Show();
        SetOpenedStateTo(true);
    }

    private void ShowButton()
    {
        deliveryNoteButton.Show();
        SetOpenedStateTo(false);
    }

    private void SetOpenedStateTo(bool isOpened)
    {
        if (currentDetails == null)
        {
            return;
        }

        deliveryNoteStates[currentDetails] = new(isOpened);
    }

    private bool GetOpenState(DeliveryDetailsSO deliveryDetails)
    {
        if (deliveryNoteStates.TryGetValue(deliveryDetails, out DeliveryNoteState state))
        {
            return state.IsOpened;
        }
        else
        {
            var noteState = new DeliveryNoteState(true);
            deliveryNoteStates.Add(deliveryDetails, noteState);
            return noteState.IsOpened;
        }
    }
}
