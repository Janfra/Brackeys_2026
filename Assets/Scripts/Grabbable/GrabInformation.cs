using Janito.EditorExtras;
using System;
using UnityEngine;

[Serializable]
public class GrabInformation
{
    public IGrabReleaseNotifier ReleaseNotifier { get; private set; }

    [field: SerializeField]
    public GameObject GrabbedObject { get; private set; }
    
    [field: SerializeField]
    public Transform Holder { get; private set; }
    
    [field: SerializeField]
    [field: Tooltip("Indicates whether the grab information is valid. A grab is considered valid if the grabbed object has not been released yet and initial values were valid.")]
    [field: ReadOnly]
    public bool IsValid { get; private set; }

    public GrabInformation(GameObject grabbedObject, Transform holder, IGrabReleaseNotifier releaseNotifier)
    {
        GrabbedObject = grabbedObject;
        Holder = holder;
        ReleaseNotifier = releaseNotifier;
        IsValid = grabbedObject != null && holder != null && releaseNotifier != null;

        if (IsValid)
        {
            ReleaseNotifier.OnReleased += OnGrabbedObjectReleased;
        }
    }

    private void OnGrabbedObjectReleased()
    {
        IsValid = false;
        ReleaseNotifier.OnReleased -= OnGrabbedObjectReleased;
    }
}