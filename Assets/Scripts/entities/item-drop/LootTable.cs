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
        int iterationCount = 0;
        while (roll > 0)
        {
            item = items[currentIndex];
            if ((item.alreadyObtained && item.canGetMultipleTimes)
                || !item.alreadyObtained)
            {
                roll -= item.dropChance;
            }

            iterationCount++;
            currentIndex = (currentIndex + 1) % items.Count;

            if (iterationCount > 99) break;
        }

        item.alreadyObtained = true;
        item.quantity = Random.Range(item.quantityRange.x, item.quantityRange.y);
        return item;
    }

    public void ResetTable()
    {
        foreach (var item in items)
        {
            item.quantity = 0;
            item.alreadyObtained = false;
        }
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