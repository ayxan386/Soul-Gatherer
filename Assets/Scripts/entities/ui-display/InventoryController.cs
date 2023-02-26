using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private Transform cellHolder;
    [SerializeField] private TextMeshProUGUI expCounter;
    [SerializeField] private TextMeshProUGUI goldCounter;
    [SerializeField] private List<SoulShard> ownedShards;
    [SerializeField] private float currentExp;
    [SerializeField] private int gold;

    [SerializeField] private Sprite tempSprite;

    private SoulShardDisplayer[] cells;
    private BaseAbility currentSelectedAbility;

    private void Start()
    {
        EventStore.Instance.OnEntityObtainedClick += OnEntityObtained;
        EventStore.Instance.OnPlayerAbilityDisplayerClick += OnAbilityDisplayerClick;
        EventStore.Instance.OnShardAdd += OnShardAdd;
        EventStore.Instance.OnShardRemove += OnShardRemove;

        if (ownedShards == null) ownedShards = new List<SoulShard>();
        cells = cellHolder.GetComponentsInChildren<SoulShardDisplayer>();
        DisplayOwnedShards();
        UpdateExpCounter();
        UpdateGoldCounter();
    }


    private void OnDestroy()
    {
        EventStore.Instance.OnEntityObtainedClick -= OnEntityObtained;
        EventStore.Instance.OnPlayerAbilityDisplayerClick -= OnAbilityDisplayerClick;
        EventStore.Instance.OnShardAdd -= OnShardAdd;
    }

    private void OnAbilityDisplayerClick(BaseAbility ability)
    {
        currentSelectedAbility = ability;
    }

    private void OnEntityObtained(object sender, ObtainedEntity e)
    {
        print("Event received: " + e.data.name);
        if (e.data.type == EntityType.Exp)
        {
            currentExp += e.count;
            UpdateExpCounter();
        }
        else if (e.data.type == EntityType.Gold)
        {
            gold += e.count;
            UpdateGoldCounter();
        }
        else if (e.data.type == EntityType.SoulShard)
        {
            ownedShards.Add(RandomShard());
            DisplayOwnedShards();
        }
    }

    private void UpdateGoldCounter()
    {
        goldCounter.text = "Gold: " + gold;
    }

    private void UpdateExpCounter()
    {
        expCounter.text = "Exp: " + currentExp.ToString("N0");
    }

    private void OnShardAdd(SoulShard soulShard)
    {
        if (soulShard != null && currentSelectedAbility != null)
        {
            currentSelectedAbility.ApplySoulShard(soulShard);
            DisplayOwnedShards();
        }
    }

    private void OnShardRemove(SoulShard soulShard)
    {
        if (soulShard != null && currentSelectedAbility != null)
        {
            currentSelectedAbility.RemoveSoulShard(soulShard);
            DisplayOwnedShards();
        }
    }

    private SoulShard RandomShard()
    {
        var res = new SoulShard();
        res.description = "Some shard";
        res.icon = tempSprite;
        return res;
    }


    private void DisplayOwnedShards()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            if (i < ownedShards.Count && ownedShards[i].attachedAbility == null)
            {
                cells[i].DisplaySoulShard(ownedShards[i]);
            }
            else
            {
                cells[i].DisplayAsLocked();
            }
        }
    }
}