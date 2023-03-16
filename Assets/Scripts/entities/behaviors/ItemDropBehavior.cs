using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class ItemDropBehavior : ItemInteractionBehavior
{
    [SerializeField] private LootTable lootTable;
    [SerializeField] private int numberOfItems;
    [SerializeField] private bool useRange;
    [SerializeField] private Vector2Int countRange;
    [SerializeField] private UnityEvent afterLootAction;

    private List<ObtainedEntity> obtainedEntities;

    private void OnDisable()
    {
        EventStore.Instance.OnEntityObtainedClick -= OnEntityObtainedClick;
    }

    private void OnEntityObtainedClick(object sender, ObtainedEntity e)
    {
        if (obtainedEntities == null || obtainedEntities.Count == 0) return;

        var index = obtainedEntities.FindIndex((temp) => temp.data.id == e.data.id);
        if (index >= 0)
            obtainedEntities.RemoveAt(index);

        if (obtainedEntities.Count <= 0)
        {
            afterLootAction?.Invoke();
            Destroy(this);
        }
    }

    [ContextMenu("Get items")]
    public override void Interact(InteractionPassData data)
    {
        Complete = false;
        if (!data.WasInteractedBefore)
            GenerateItemFromLootTable();

        DisplayItems();
        EventStore.Instance.OnEntityObtainedClick += OnEntityObtainedClick;
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

[Serializable]
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