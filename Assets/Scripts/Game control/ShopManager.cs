using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Transform abilityHolder;
    [SerializeField] private Transform relicHolder;
    private AbilityDisplayer[] abilitySlots;

    void Start()
    {
        abilitySlots = abilityHolder.GetComponentsInChildren<AbilityDisplayer>();
        EventStore.Instance.OnShopOpen += OnShopOpen;
    }

    private void OnDestroy()
    {
        EventStore.Instance.OnShopOpen -= OnShopOpen;
    }

    private void OnShopOpen(ShopDisplayData obj)
    {
        for (int dataIndex = 0; dataIndex < obj.abilities.Count; dataIndex++)
        {
            if (dataIndex < abilitySlots.Length)
            {
            }
        }
    }

    void Update()
    {
    }
}

[Serializable]
public class ShopDisplayData
{
    public Dictionary<string, ShopAbilityData> abilities;
    public List<ShopRelicData> relics;
}

public class ShopAbilityData
{
    public int price;
    public bool isBougth;
}

public class ShopRelicData
{
    public BaseRelic relic;
    public int price;
}