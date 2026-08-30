using System.Collections.Generic;
using UnityEngine;

public static class CollisionExtensions
{
    /// <summary>
    /// Attempts to calculate the middle point of the collision by averaging the collisions points.
    /// </summary>
    /// <param name="collision">Collision containing the information</param>
    /// <param name="contactPoints">List to store the contact point information</param>
    /// <param name="middlePoint">Calculated middle point of the collision</param>
    /// <returns>True if a middle point was calculated, otherwise false</returns>
    /// <exception cref="System.ArgumentNullException"><c>collision</c> or <c>contactPoints</c> references were null</exception>
    public static bool TryGetMiddlePoint(this Collision collision, List<ContactPoint> contactPoints, out Vector3 middlePoint)
    {
        if (collision == null)
        {
            throw new System.ArgumentNullException($"Collision cannot be null in {nameof(TryGetMiddlePoint)} extension method.");
        }

        if (contactPoints == null)
        {
            throw new System.ArgumentNullException($"Contact points list cannot be null in {nameof(TryGetMiddlePoint)} extension method.");
        }

        var contactCount = collision.GetContacts(contactPoints);
        if (contactCount == 0)
        {
            middlePoint = Vector3.zero;
            return false;
        }

        if (contactCount == 1)
        {
            middlePoint = contactPoints[0].point;
            return true;
        }

        Vector3 sum = Vector3.zero;
        foreach (var contact in contactPoints)
        {
            sum += contact.point;
        }

        middlePoint = sum / contactPoints.Count;
        return true;
    }
}
