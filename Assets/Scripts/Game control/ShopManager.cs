using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Transform abilityHolder;
    [SerializeField] private Transform relicHolder;
    [SerializeField] private GameObject shopUi;
    private AbilityDisplayer[] abilitySlots;

    private ShopDisplayData currentData;

    void Start()
    {
        abilitySlots = abilityHolder.GetComponentsInChildren<AbilityDisplayer>();
        EventStore.Instance.OnShopOpen += OnShopOpen;
        EventStore.Instance.OnPlayerAbilityDisplayerClick += OnPlayerAbilityDisplayerClick;
        EventStore.Instance.OnGoldChanged += OnGoldChanged;
    }

    private void OnDestroy()
    {
        EventStore.Instance.OnShopOpen -= OnShopOpen;
        EventStore.Instance.OnPlayerAbilityDisplayerClick -= OnPlayerAbilityDisplayerClick;
        EventStore.Instance.OnGoldChanged -= OnGoldChanged;
    }

    private void OnShopOpen(ShopDisplayData shopData)
    {
        GlobalStateManager.Instance.PausedGame("Shop");
        shopUi.SetActive(true);
        currentData = shopData;

        UpdateCurrentUi();
    }

    private void UpdateCurrentUi()
    {
        var slotIndex = 0;
        foreach (var abilityData in currentData.abilities)
        {
            if (slotIndex < abilitySlots.Length)
            {
                var abilityDisplayer = abilitySlots[slotIndex];
                slotIndex++;
                abilityDisplayer.price = abilityData.Value.price;
                abilityDisplayer.DisplayAbility(PlayerAbilityReferenceKeeper.PlayerAbilities[abilityData.Key]);
            }
        }
    }

    private void OnPlayerAbilityDisplayerClick(AbilityDisplayer clickedAbility)
    {
        if (clickedAbility.type != AbilityDisplayType.ShopMenu) return;

        if (EventStore.Instance.GetCurrentGoldFromInventory().GetValueOrDefault(0) >= clickedAbility.price)
        {
            EventStore.Instance.PublishGoldSpent(clickedAbility.price);
        }
    }


    private void OnGoldChanged(int totalGold)
    {
        UpdateCurrentUi();
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