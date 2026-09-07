using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HouseDetails : IHouseDetails
{
    [field: SerializeField]
    public Vector3 DeliveryPoint { get; set; }

    private DescriptionsConfigurationSO descriptionsConfiguration;
    private Queue<string> unusedDescriptions = new();

    public string DisplayName => descriptionsConfiguration.DisplayName;

    public HouseDetails(Vector3 deliveryPoint, DescriptionsConfigurationSO descriptionsConfiguration)
    {
        DeliveryPoint = deliveryPoint;
        SetDescriptionConfiguration(descriptionsConfiguration);
    }

    public void SetDescriptionConfiguration(DescriptionsConfigurationSO descriptionsConfiguration)
    {
        if (unusedDescriptions == null)
        {
            unusedDescriptions = new Queue<string>();
        }

        this.descriptionsConfiguration = descriptionsConfiguration;
        SetDescriptionQueueToConfig();
    }

    public string GetDescription()
    {
        if (unusedDescriptions.Count == 0)
        {
            SetDescriptionQueueToConfig();
        }

        return unusedDescriptions.Count > 0 ? unusedDescriptions.Dequeue() : string.Empty;
    }

    private void SetDescriptionQueueToConfig()
    {
        if (descriptionsConfiguration == null)
        {
            return;
        }

        if (unusedDescriptions.Count > 0)
        {
            unusedDescriptions.Clear();
        }

        foreach (var description in descriptionsConfiguration.Descriptions)
        {
            unusedDescriptions.Enqueue(description);
        }
    }
}
