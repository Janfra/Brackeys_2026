using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Descriptions Configuration", menuName = "Scriptable Objects/Configuration/Descriptions")]
public class DescriptionsConfigurationSO : ScriptableObject
{
    [field: SerializeField]
    public string DisplayName { get; private set; }

    [field: SerializeField]
    [field: TextArea]
    public List<string> Descriptions { get; private set; }
}
