using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShardInventoryController : MonoBehaviour
{
    [SerializeField] private Transform cellHolder;
    [SerializeField] private TextMeshProUGUI goldCounter;
    [SerializeField] private int gold;

    [SerializeField] private List<ShardDropData> possibleShards;

    [SerializeField] private List<SoulShard> ownedShards;
    private SoulShardDisplayer[] cells;
    private BaseAbility currentSelectedAbility;

    private void Start()
    {
        EventStore.Instance.OnEntityObtainedClick += OnEntityObtained;
        EventStore.Instance.OnPlayerAbilityDisplayerClick += OnAbilityDisplayerClick;
        EventStore.Instance.OnShardAdd += OnShardAdd;
        EventStore.Instance.OnShardRemove += OnShardRemove;
        EventStore.Instance.OnPlayerDataLoad += OnPlayerDataLoad;
        EventStore.Instance.OnPlayerDataSave += OnPlayerDataSave;

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
        EventStore.Instance.OnShardRemove -= OnShardRemove;
        EventStore.Instance.OnPlayerDataLoad -= OnPlayerDataLoad;
        EventStore.Instance.OnPlayerDataSave -= OnPlayerDataSave;
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


    private void OnPlayerDataSave(PlayerWorldData obj)
    {
        if (obj.abilities == null) obj.abilities = new List<PlayerAbilityData>();
        foreach (var abilitiesValue in PlayerAbilityReferenceKeeper.PlayerAbilities.Values)
        {
            obj.abilities.Add(abilitiesValue.GetData());
        }

        obj.ownedShards = ownedShards;
    }

    private void OnPlayerDataLoad(PlayerWorldData savedData)
    {
        if (savedData.abilities == null) return;
        ownedShards = new List<SoulShard>(savedData.ownedShards);

        foreach (var ownedShard in ownedShards)
        {
            if (ownedShard.attached && !string.IsNullOrEmpty(ownedShard.abilityId))
            {
                var playerAbility = PlayerAbilityReferenceKeeper.PlayerAbilities[ownedShard.abilityId];
                var savedPlayerAbility =
                    savedData.abilities.Find((savedAbility) => savedAbility.id == ownedShard.abilityId);
                if (savedPlayerAbility != null)
                {
                    playerAbility.ApplyData(savedPlayerAbility);
                    playerAbility.ApplySoulShard(ownedShard);
                }
            }
        }

        foreach (var abilityData in savedData.abilities)
        {
            var playerAbility = PlayerAbilityReferenceKeeper.PlayerAbilities[abilityData.id];
            playerAbility.ApplyData(abilityData);
        }


        DisplayOwnedShards();
    }

    private void UpdateGoldCounter()
    {
        goldCounter.text = "Gold: " + gold;
    }

    private void OnShardAdd(SoulShard soulShard)
    {
        if (soulShard != null && currentSelectedAbility != null
                              && currentSelectedAbility.CanBeModified
                              && currentSelectedAbility.CanApplySoulShard(soulShard))
        {
            currentSelectedAbility.ApplySoulShard(soulShard);
            DisplayOwnedShards();
        }
    }

    private void OnShardRemove(SoulShard soulShard)
    {
        print("Remove event received");
        if (soulShard != null && currentSelectedAbility != null)
        {
            print("Remove event received");
            currentSelectedAbility.RemoveSoulShard(soulShard);
            soulShard.attached = false;
            DisplayOwnedShards();
        }
    }

    private SoulShard RandomShard()
    {
        var res = new SoulShard();
        Random.InitState(Guid.NewGuid().ToString().GetHashCode());
        var range = 0;
        for (int i = 0; i < Random.Range(10, 20); i++)
        {
            range = Random.Range(0, Enum.GetNames(typeof(SoulShardType)).Length);
        }

        var soulShardType = (SoulShardType)range;

        soulShardType = SoulShardType.AOE_Coverage;

        var shardDropData = possibleShards.Find(data => data.type == soulShardType);
        if (shardDropData == null) throw new ArgumentException("Type not found");
        shardDropData.ApplyTo(res);
        return res;
    }

    private void DisplayOwnedShards()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            if (i < ownedShards.Count)
            {
                print("Updated ui for : " + ownedShards[i]);
            }

            if (i < ownedShards.Count && !ownedShards[i].attached)
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