﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RelicInventoryController : MonoBehaviour
{
    [SerializeField] private List<BaseRelic> ownedRelics;
    [SerializeField] private List<BaseRelic> commonRelics;
    [SerializeField] private List<BaseRelic> rareRelics;
    [SerializeField] private List<BaseRelic> epicRelics;

    public static RelicInventoryController Instance { get; private set; }

    public List<BaseRelic> OwnedRelics => ownedRelics;

    private void Awake()
    {
        Instance = this;
        ownedRelics = new List<BaseRelic>();
    }

    private void Start()
    {
        EventStore.Instance.OnRelicObtained += OnRelicObtained;
        EventStore.Instance.OnEntityObtainedClick += OnEntityObtainedClick;
    }


    private void OnDestroy()
    {
        EventStore.Instance.OnRelicObtained -= OnRelicObtained;
        EventStore.Instance.OnEntityObtainedClick -= OnEntityObtainedClick;
    }

    private void OnRelicObtained(BaseRelic newRelic)
    {
        if (CanObtainRelic(newRelic))
        {
            AddRelicToInventory(newRelic);
        }
        else
        {
            Destroy(newRelic.gameObject);
        }
    }

    private bool CanObtainRelic(BaseRelic newRelic)
    {
        if (newRelic.CanHaveMultiple)
        {
            return true;
        }

        var index = ownedRelics.FindIndex(relic => relic.Name == newRelic.Name);
        return index < 0;
    }


    private void OnEntityObtainedClick(object sender, ObtainedEntity e)
    {
        if (e.data.type is not (EntityType.Relic_Common or EntityType.Relic_Rare or EntityType.Relic_Epic)) return;

        EventStore.Instance.PublishRelicObtained(GenerateRandomRelic(e.data.type));
    }

    private BaseRelic GenerateRandomRelic(EntityType relicType)
    {
        var relicsSource = commonRelics;
        if (relicType == EntityType.Relic_Rare)
        {
            relicsSource = rareRelics;
        }
        else if (relicType == EntityType.Relic_Epic)
        {
            relicsSource = epicRelics;
        }

        Random.InitState(Time.time.ToString().GetHashCode());
        var roll = Random.value;
        int index = 0;
        while (roll > 0)
        {
            if (CanObtainRelic(relicsSource[index % relicsSource.Count]))
                roll -= relicsSource[index % relicsSource.Count].DropChance;
            if (roll > 0)
                index++;

            if (index > 100)
            {
                throw new ArgumentException("Relic not found");
            }
        }

        return Instantiate(relicsSource[index % relicsSource.Count]);
    }

    private void AddRelicToInventory(BaseRelic obj)
    {
        print("Added new relic: " + obj.Name);
        ownedRelics.Add(obj);
        obj.RelicObtained();
        obj.transform.parent = transform;
    }
}