using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Transform abilityHolder;
    [SerializeField] private Transform relicHolder;
    [SerializeField] private GameObject shopUi;
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

    private void OnShopOpen(ShopDisplayData shopData)
    {
        GlobalStateManager.Instance.PausedGame("Shop");
        shopUi.SetActive(true);
        var slotIndex = 0;
        foreach (var abilityData in shopData.abilities)
        {
            if (slotIndex < abilitySlots.Length)
            {
                var abilityDisplayer = abilitySlots[slotIndex];
                slotIndex++;
                abilityDisplayer.DisplayAbility(PlayerAbilityReferenceKeeper.PlayerAbilities[abilityData.Key]);
                abilityDisplayer.price = abilityData.Value.price;
            }
        }
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