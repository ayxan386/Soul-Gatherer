using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopBehavior : ItemInteractionBehavior, ILoadableEntity
{
    [SerializeField] private ShopDisplayData shopData;
    private StringIdHolder assignedId;

    private void Awake()
    {
        assignedId = GetComponent<StringIdHolder>();
    }

    public override void Interact(InteractionPassData data)
    {
        if (!data.WasInteractedBefore)
        {
            shopData = new ShopDisplayData();
            shopData.abilities = new HashSet<ShopAbilityData>();
            foreach (var keyValuePair in PlayerAbilityReferenceKeeper.PlayerAbilities)
            {
                if (keyValuePair.Value.CanBeModified) continue;

                var shopAbilityData = new ShopAbilityData();
                shopAbilityData.price = (keyValuePair.Value.AvailableSlots + 1) * 5;
                shopAbilityData.id = keyValuePair.Key;
                shopData.abilities.Add(shopAbilityData);
            }
        }

        EventStore.Instance.PublishShopOpen(shopData);
        Complete = true;
    }

    public void LoadData(LoadableEntityData data)
    {
        if (data == null || !data.shopData.isGenerated) return;
        shopData = data.shopData;
        shopData.abilities = new HashSet<ShopAbilityData>(shopData.serializedAbilities);
    }

    public LoadableEntityData GetData()
    {
        if (shopData == null) return null;
        var entityData = new LoadableEntityData();
        if (shopData.abilities != null)
        {
            shopData.serializedAbilities = shopData.abilities.ToList();
            shopData.isGenerated = true;
        }

        entityData.shopData = shopData;
        entityData.instanceId = GetId();
        return entityData;
    }

    public void SetId(string id)
    {
        assignedId = gameObject.AddComponent<StringIdHolder>();
        assignedId.id = id;
    }

    public string GetId()
    {
        return assignedId.id + GetType().Name;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}