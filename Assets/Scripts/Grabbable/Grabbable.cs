using Janito.EditorExtras;
using System;
using UnityEngine;

public interface IGrabbrableSource
{
    public Transform Transform { get; }
    public Rigidbody Rigidbody { get; }
}

[Serializable]
public class Grabbable
{
    [CreateButton(savePath: PathUtils.ProjectConfigurationPath + "/Grab")]
    [InlineInspector]
    public GrabConfigurationSO GrabConfiguration;

    private IGrabbrableSource source;

    public Transform Holder { get; private set; }
    public bool IsValid => source != null && source.Transform != null && source.Rigidbody != null;

    public Grabbable(IGrabbrableSource source)
    {
        Initialize(source);
    }

    public Grabbable(IGrabbrableSource source, GrabConfigurationSO grabConfiguration)
    {
        Initialize(source);
        GrabConfiguration = grabConfiguration;
    }

    public void Initialize(IGrabbrableSource source)
    {
        this.source = source;
    }

    public void Grab(Transform newHolder)
    {
        if (newHolder == null)
        {
            return;
        }

        if (Holder != null)
        {
            Release();
        }

        Holder = newHolder;
        MoveOnTopOfObject(Holder);
        source.Transform.SetParent(Holder); // Match holder position
        DisablePhysics();
    }

    public void Release()
    {
        source.Transform.SetParent(null);
        EnablePhysics();
        Holder = null;
    }

    public void Throw()
    {
        Throw(GrabConfiguration.ThrowForce);
    }

    public void Throw(DirectionalThrowForce force)
    {
        if (Holder == null)
        {
            return;
        }

        Transform oldHolder = Holder;
        Release();
        Vector3 relativeVelocity = GetAdditionalVelocityFromHolder(oldHolder);
        Vector3 throwDirection = oldHolder.transform.forward * force.ForwardStrength + oldHolder.transform.up * force.UpwardStrength;
        source.Rigidbody.AddForce(throwDirection + relativeVelocity, ForceMode.Impulse);
    }

    private void MoveOnTopOfObject(Transform target)
    {
        var position = target.position;
        position.y += GrabConfiguration.HeightOffset;

        if (TryGetHeightFromCollider(target, out float yOffset))
        {
            position.y += yOffset;
        }
        else
        {
            LogLibrary.LogWarningInDevelopment<Grabbable>($"Unable to determine size of source from collider on {target.gameObject.name}. Using arbitrary offset instead.");
            position.y += GrabConfiguration.HeightOffset; // Do height offset again as a fallback for now to try to put it on top
        }

        source.Transform.position = position;
        source.Transform.rotation = target.rotation;
    }

    private void DisablePhysics()
    {
        // Clear any remaining velocities
        source.Rigidbody.angularVelocity = Vector3.zero;
        source.Rigidbody.linearVelocity = Vector3.zero;
        source.Rigidbody.isKinematic = true;
    }

    private void EnablePhysics()
    {
        source.Rigidbody.isKinematic = false;
    }

    private Vector3 GetAdditionalVelocityFromHolder(Transform holder)
    {
        if (holder.TryGetComponent(out Rigidbody rb))
        {
            return rb.linearVelocity;
        }
        return Vector3.zero;
    }

    private bool TryGetHeightFromCollider(Transform target, out float height)
    {
        if (target.TryGetComponent(out Collider collider))
        {
            height = collider.bounds.extents.y;
            return true;
        }

        height = 0.0f;
        return false;
    }
}
