using UnityEngine;

public class ItemDropBehavior : ItemInteractionBehavior
{
    [SerializeField] private LootTable lootTable;
    [SerializeField] private int numberOfItems;
    [SerializeField] private bool useRange;
    [SerializeField] private Vector2Int countRange;

    [ContextMenu("Get items")]
    public override void Interact(InteractionPassData data)
    {
        if(data.WasInteractedBefore) return;
        var count = numberOfItems;
        if (useRange)
        {
            count = Random.Range(countRange.x, countRange.y);
        }

        for (int i = 0; i < count; i++)
        {
            var item = lootTable.GetItem(Random.value);
            print(item);
        }
    }
}