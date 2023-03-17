using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private Transform cellHolder;
    [SerializeField] private TextMeshProUGUI goldCounter;
    [SerializeField] private List<SoulShard> ownedShards;
    [SerializeField] private int gold;

    [SerializeField] private List<ShardDropData> possibleShards;

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
        UpdateGoldCounter();
    }


    private void OnDestroy()
    {
        EventStore.Instance.OnEntityObtainedClick -= OnEntityObtained;
        EventStore.Instance.OnPlayerAbilityDisplayerClick -= OnAbilityDisplayerClick;
        EventStore.Instance.OnShardAdd -= OnShardAdd;
    }

    private void OnAbilityDisplayerClick(AbilityDisplayer displayer)
    {
        if (displayer.type != AbilityDisplayType.ModificationMenu)
            return;
        var ability = PlayerAbilityReferenceKeeper.PlayerAbilities[displayer.id];
        currentSelectedAbility = ability;
    }

    private void OnEntityObtained(object sender, ObtainedEntity e)
    {
        if (e.data.type == EntityType.Gold)
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
        Random.InitState(Time.time.ToString().GetHashCode());
        var range = Random.Range(0, 5);
        for (int i = 0; i < Random.Range(10, 50); i++)
        {
            range = Random.Range(0, 6);
        }

        var soulShardType = (SoulShardType)range;

        var shardDropData = possibleShards.Find(data => data.type == soulShardType);
        if (shardDropData == null) throw new ArgumentException("Type not found");
        shardDropData.ApplyTo(res);
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