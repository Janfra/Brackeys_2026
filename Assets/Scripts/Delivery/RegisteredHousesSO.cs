using Janito.EditorExtras;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Registered Houses (Unique)", menuName = "Scriptable Objects/Delivery/Registered Houses")]
public class RegisteredHousesSO : ScriptableObject
{
    private List<HouseSO> registeredHouses;

    public IReadOnlyList<HouseSO> RegisteredHouses => registeredHouses;

    public void RegisterHouse(HouseSO house)
    {
        if (registeredHouses.Contains(house))
        {
            this.LogWarningInDevelopment($"Attempting to register house '{house.name}' twice");
            return;
        }

        registeredHouses.Add(house);
    }

    public void UnregisterHouse(HouseSO house)
    {
        registeredHouses.Remove(house);
    }

    public HouseSO CreateAndRegisterHouse()
    {
        var newHouse = CreateInstance<HouseSO>();
        newHouse.name = $"House #{registeredHouses.Count}";
        RegisterHouse(newHouse);

        return newHouse;
    }

    private void OnEnable()
    {
        registeredHouses = new();
    }
}
