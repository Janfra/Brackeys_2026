using Janito.EditorExtras;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Registered Houses (Unique)", menuName = "Scriptable Objects/Delivery/Registered Houses")]
public class RegisteredHousesSO : ScriptableObject
{
    private List<HouseSO> registeredHouses;
    private Dictionary<HouseSO, HouseDetails> houseDetailsMap;

    public IReadOnlyList<HouseSO> RegisteredHouses => registeredHouses;

    private void OnEnable()
    {
        registeredHouses = new();
        houseDetailsMap = new();
    }

    private void OnDisable()
    {
        registeredHouses = new();
        houseDetailsMap = new();
    }

    public bool TryGetHouseDetails(HouseSO house, out HouseDetails details)
    {
        return houseDetailsMap.TryGetValue(house, out details);
    }

    public void RegisterHouse(HouseSO house, HouseDetails details)
    {
        if (registeredHouses.Contains(house))
        {
            this.LogWarningInDevelopment($"Attempting to register house '{house.name}' twice");
            return;
        }

        RegisterHouseDetails(house, details);
        registeredHouses.Add(house);
    }

    public void UnregisterHouse(HouseSO house)
    {
        registeredHouses.Remove(house);
    }

    public HouseSO CreateAndRegisterHouse(HouseDetails details)
    {
        var newHouse = CreateInstance<HouseSO>();
        newHouse.name = $"House #{registeredHouses.Count + 1}";
        RegisterHouseDetails(newHouse, details);
        RegisterHouse(newHouse, details);
        return newHouse;
    }

    private void RegisterHouseDetails(HouseSO house, HouseDetails details)
    {
        if (string.IsNullOrEmpty(details.DisplayName))
        {
            details.DisplayName = house.name;
        }

        if (houseDetailsMap.ContainsKey(house))
        {
            houseDetailsMap[house] = details;
        }
        else
        {
            houseDetailsMap.Add(house, details);
        }
    }
}
