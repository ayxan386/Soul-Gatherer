using System.Collections.Generic;
using UnityEngine;

public class ItemDropBehavior : ItemInteractionBehavior
{
    [SerializeField] private LootTable lootTable;
    [SerializeField] private int numberOfItems;
    [SerializeField] private bool useRange;
    [SerializeField] private Vector2Int countRange;

    private List<ObtainedEntity> obtainedEntities;

    [ContextMenu("Get items")]
    public override void Interact(InteractionPassData data)
    {
        Complete = false;
        if (!data.WasInteractedBefore)
            GenerateItemFromLootTable();

        DisplayItems();
        Complete = true;
    }

    private void DisplayItems()
    {
        foreach (var entity in obtainedEntities)
        {
            print(entity.data.name + " " + entity.count);
        }
    }

    private void GenerateItemFromLootTable()
    {
        var count = numberOfItems;
        if (useRange)
        {
            count = Random.Range(countRange.x, countRange.y);
        }

        obtainedEntities = new List<ObtainedEntity>(count);

        for (int i = 0; i < count; i++)
        {
            var item = lootTable.GetItem(Random.value);
            obtainedEntities.Add(new ObtainedEntity(item));
        }

        lootTable.ResetTable();
    }
}

public class ObtainedEntity
{
    public EntityData data;
    public int count;
    public bool consumed;

    public ObtainedEntity(EntityData data, int count)
    {
        this.data = data;
        this.count = count;
        consumed = false;
    }

    public ObtainedEntity(LootableItem item) : this(item.entity, item.quantity)
    {
    }
}