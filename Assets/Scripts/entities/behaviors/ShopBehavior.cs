using System.Collections.Generic;

public class ShopBehavior : ItemInteractionBehavior
{
    public override void Interact(InteractionPassData data)
    {
        var shopData = new ShopDisplayData();
        shopData.abilities = new Dictionary<string, ShopAbilityData>();
        foreach (var keyValuePair in PlayerAbilityReferenceKeeper.PlayerAbilities)
        {
            if (keyValuePair.Value.CanBeModified) continue;

            var shopAbilityData = new ShopAbilityData();
            shopAbilityData.price = keyValuePair.Value.AvailableSlots * 5;
            shopData.abilities.Add(keyValuePair.Key, shopAbilityData);
        }

        EventStore.Instance.PublishShopOpen(shopData);
    }
}