using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Transform abilityHolder;
    [SerializeField] private Transform relicHolder;
    [SerializeField] private GameObject shopUi;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Button closeShopButton;
    private AbilityDisplayer[] abilitySlots;

    private ShopDisplayData currentData;
    private int gold;

    void Start()
    {
        abilitySlots = abilityHolder.GetComponentsInChildren<AbilityDisplayer>();
        closeShopButton.onClick.AddListener(() => CloseShop());
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
        SelectionController.FindNextSelectable();
        currentData = shopData;

        UpdateCurrentUi();
    }

    private void UpdateCurrentUi()
    {
        if (currentData == null) return;
        gold = EventStore.Instance.GetCurrentGoldFromInventory().GetValueOrDefault(0);
        var slotIndex = 0;
        foreach (var abilityData in currentData.abilities)
        {
            if (slotIndex < abilitySlots.Length)
            {
                var abilityDisplayer = abilitySlots[slotIndex];
                slotIndex++;
                if (abilityData.isBougth)
                {
                    abilityDisplayer.price = "Sold out";
                    abilityDisplayer.priceTextColor = Color.black;
                }
                else
                {
                    abilityDisplayer.price = $"{abilityData.price}G";
                    abilityDisplayer.priceTextColor = gold >= abilityData.price ? Color.black : Color.red;
                }

                abilityDisplayer.DisplayAbility(PlayerAbilityReferenceKeeper.PlayerAbilities[abilityData.id]);
            }
        }

        goldText.text = $"Gold : {gold} G";
    }

    private void OnPlayerAbilityDisplayerClick(AbilityDisplayer clickedAbility)
    {
        if (clickedAbility.type != AbilityDisplayType.ShopMenu) return;
        var shopAbilityData = new ShopAbilityData();
        shopAbilityData.id = clickedAbility.id;
        currentData.abilities.TryGetValue(shopAbilityData, out shopAbilityData);
        var price = shopAbilityData.price;
        if (gold >= price)
        {
            shopAbilityData.isBougth = true;
            PlayerAbilityReferenceKeeper.PlayerAbilities[clickedAbility.id].ExpandSlotCount();
            EventStore.Instance.PublishGoldSpent(price);
        }
    }


    private void OnGoldChanged(int totalGold)
    {
        gold = totalGold;
        UpdateCurrentUi();
    }

    private void CloseShop()
    {
        currentData = null;
        shopUi.SetActive(false);
        GlobalStateManager.Instance.RunningGame("Shop");
    }
}

[Serializable]
public class ShopDisplayData
{
    public HashSet<ShopAbilityData> abilities;
    public List<ShopAbilityData> serializedAbilities;
    public List<ShopRelicData> relics;
    public bool isGenerated = false;
}

[Serializable]
public class ShopAbilityData
{
    public string id;
    public int price;
    public bool isBougth;

    public override bool Equals(object obj)
    {
        if (obj is not ShopAbilityData) return false;
        var other = obj as ShopAbilityData;
        return other.id == this.id;
    }

    public override int GetHashCode()
    {
        return id.GetHashCode();
    }
}

[Serializable]
public class ShopRelicData
{
    public BaseRelic relic;
    public int price;
}