using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class ItemDropBehavior : ItemInteractionBehavior, ILoadableEntity
{
    [SerializeField] private LootTable lootTable;
    [SerializeField] private int numberOfItems;
    [SerializeField] private bool useRange;
    [SerializeField] private Vector2Int countRange;
    [SerializeField] private UnityEvent afterLootAction;
    private StringIdHolder assignedId;

    private List<ObtainedEntity> obtainedEntities;

    private void Awake()
    {
        assignedId = GetComponent<StringIdHolder>();
    }

    private void OnDisable()
    {
        EventStore.Instance.OnEntityObtainedClick -= OnEntityObtainedClick;
    }

    private void OnEntityObtainedClick(object sender, ObtainedEntity e)
    {
        if (obtainedEntities == null || obtainedEntities.Count == 0 || e.attachedId != assignedId.id) return;

        var index = obtainedEntities.FindIndex((temp) =>
            temp.data.id == e.data.id && temp.count == e.count);
        if (index >= 0)
            obtainedEntities.RemoveAt(index);

        if (obtainedEntities.Count <= 0)
        {
            Complete = true;
            afterLootAction?.Invoke();
        }
    }

    [ContextMenu("Get items")]
    public override void Interact(InteractionPassData data)
    {
        print("Item drop interaction");
        if (!data.WasInteractedBefore)
            GenerateItemFromLootTable();

        DisplayItems();
        EventStore.Instance.OnEntityObtainedClick += OnEntityObtainedClick;
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
        Random.InitState(Time.time.ToString().GetHashCode());
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
            print("Generated new item: " + obtainedEntity.data.name);
            obtainedEntity.attachedId = assignedId.id;
            obtainedEntities.Add(obtainedEntity);
        }

        lootTable.ResetTable();
    }

    public void LoadData(LoadableEntityData data)
    {
        obtainedEntities = data.obtainedEntities;
    }

    public LoadableEntityData GetData()
    {
        var entityData = new LoadableEntityData();
        entityData.instanceId = GetId();
        entityData.obtainedEntities = obtainedEntities;
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

[Serializable]
public class ObtainedEntity
{
    public EntityData data;
    public int count;
    public bool consumed;
    public string attachedId;

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