using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LootTable", order = 1)]
public class LootTable : ScriptableObject
{
    [SerializeField] private List<LootableItem> items;

    [ContextMenu("Loot item")]
    public LootableItem GetItem(float roll)
    {
        int currentIndex = 0;
        var item = items[currentIndex];
        while (roll > 0)
        {
            if (item.canGetMultipleTimes ^ item.alreadyObtained)
            {
                roll -= item.dropChance;
            }

            currentIndex = (currentIndex + 1) % items.Count;
        }

        item.alreadyObtained = true;
        item.quantity = Random.Range(item.quantityRange.x, item.quantityRange.y);
        return item;
    }
}

[Serializable]
public class LootableItem
{
    public EntityData entity;
    [Range(0, 1f)] public float dropChance;
    public int quantity;
    public Vector2Int quantityRange;
    public bool canGetMultipleTimes = true;
    public bool alreadyObtained = false;

    public override string ToString()
    {
        return $"{entity.name} => count{quantity}";
    }
}