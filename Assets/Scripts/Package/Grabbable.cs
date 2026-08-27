using Janito.EditorExtras;
using System;
using UnityEngine;

[Serializable]
public class Grabbable
{
    public Transform Transform; 
    public Rigidbody Rigidbody;

    [CreateButton(savePath: PathUtils.ProjectConfigurationPath + "/Grab")]
    [InlineInspector]
    public GrabConfigurationSO GrabConfiguration;

    public Transform Holder { get; private set; }

    public bool IsValid => Transform != null && Rigidbody != null;

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
        Transform.SetParent(Holder); // Match holder position
        DisablePhysics();
    }

    public void Release()
    {
        Transform.SetParent(null);
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
        Rigidbody.AddForce(throwDirection + relativeVelocity, ForceMode.Impulse);
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

        Transform.position = position;
        Transform.rotation = target.rotation;
    }

    private void DisablePhysics()
    {
        Rigidbody.isKinematic = true;
    }

    private void EnablePhysics()
    {
        Rigidbody.isKinematic = false;
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
