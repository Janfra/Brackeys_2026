using System;
using UnityEngine;

public class PackageDetailsSO : ScriptableObject, IDisposable
{
    public HouseSO DeliveryHouse;
    public Timer ExpirationTimer;

    public IReadOnlyTimer ReadOnlyExpirationTimer => ExpirationTimer;

    public void Dispose()
    {
        ExpirationTimer.Dispose();
    }
}
