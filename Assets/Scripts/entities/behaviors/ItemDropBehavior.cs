using System.Collections.Generic;
using UnityEngine;

public class ItemDropBehavior : ItemInteractionBehavior
{
    [SerializeField] private LootTable lootTable;
    [SerializeField] private int numberOfItems;
    [SerializeField] private bool useRange;
    [SerializeField] private Vector2Int countRange;

    private List<ObtainedEntity> obtainedEntities;

    private void Start()
    {
        EventStore.Instance.OnEntityObtainedClick += OnOnEntityObtainedClick;
    }

    private void OnDisable()
    {
        EventStore.Instance.OnEntityObtainedClick -= OnOnEntityObtainedClick;
    }

    private void OnOnEntityObtainedClick(object sender, ObtainedEntity e)
    {
        print("TODO add to the inventory...");
        var index = obtainedEntities.FindIndex((temp) => temp.data.id == e.data.id);
        obtainedEntities.RemoveAt(index);
    }

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
            EventStore.Instance.PublishEntityObtainedDisplay(entity);
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
            var obtainedEntity = new ObtainedEntity(item);
            obtainedEntities.Add(obtainedEntity);
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