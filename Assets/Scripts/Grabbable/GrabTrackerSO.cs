using Janito.EditorExtras;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Grab Tracker", menuName = "Scriptable Objects/Trackers/Grab Tracker")]
public class GrabTrackerSO : ScriptableObject, IGrabTracker
{
    public event UnityAction<GrabInformation> OnNewGrabbed;

    [field: SerializeField]
    [field: ReadOnly]
    public GrabInformation LastGrabbed { get; private set; }

    public void NotifyNewGrabbed(GrabInformation grabData)
    {
        LastGrabbed = grabData;
        OnNewGrabbed?.Invoke(grabData);
    }
}
