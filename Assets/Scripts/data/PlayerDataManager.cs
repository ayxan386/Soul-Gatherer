using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class PlayerDataManager
{
    public const string playerSaveFileName = "player-data.json";
    public const string entitySaveFileName = "entities-data.json";

    public static void SaveData()
    {
        SavePlayerData();
        SaveEntityData();
    }

    private static void SavePlayerData()
    {
        var playerWorldData = new PlayerWorldData();
        EventStore.Instance.PublishPlayerWorldDataPrepared(playerWorldData);
        playerWorldData.level = LevelLoader.Instance.GetCurrentLevel().order;
        playerWorldData.campaignId = LevelLoader.Instance.GetCampaignId();

        var json = JsonUtility.ToJson(playerWorldData);
        var path = Path.Combine(Application.persistentDataPath, playerSaveFileName);
        File.WriteAllText(path, json);
    }

    private static void SaveEntityData()
    {
        //Entities

        var loadableEntities = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<ILoadableEntity>();
        var data = new LoadableEntityWrapper();
        data.datas = new List<LoadableEntityData>();
        foreach (var loadableEntity in loadableEntities)
        {
            data.datas.Add(loadableEntity.GetData());
        }

        data.level = LevelLoader.Instance.GetCurrentLevel().order;
        data.campaignId = LevelLoader.Instance.GetCampaignId();

        var json = JsonUtility.ToJson(data);
        var path = Path.Combine(Application.persistentDataPath, entitySaveFileName);
        File.WriteAllText(path, json);
    }

    public static void LoadData()
    {
        LoadPlayerData();
        LoadEntityData();
    }

    private static void LoadPlayerData()
    {
        var path = Path.Combine(Application.persistentDataPath, playerSaveFileName);
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            var playerWorldData = JsonUtility.FromJson<PlayerWorldData>(json);
            if (playerWorldData.campaignId == LevelLoader.Instance.GetCampaignId())
            {
                EventStore.Instance.PublishPlayerWorldDataLoaded(playerWorldData);
            }
        }
    }

    private static void LoadEntityData()
    {
        var path = Path.Combine(Application.persistentDataPath, entitySaveFileName);
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            var entityWrapper = JsonUtility.FromJson<LoadableEntityWrapper>(json);
            if (entityWrapper.campaignId == LevelLoader.Instance.GetCampaignId())
            {
                var loadableEntities = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<ILoadableEntity>();
                foreach (var loadableEntity in loadableEntities)
                {
                    var res = entityWrapper.datas.Find(data => data.instanceId == loadableEntity.GetId());
                    if (res != null)
                    {
                        loadableEntity.LoadData(res);
                    }
                    else
                    {
                        loadableEntity.Destroy();
                    }
                }
            }
        }
    }
}

[Serializable]
public class PlayerWorldData
{
    public string campaignId;
    public Vector3 position;
    public Quaternion rotation;
    public int level;
    public float speed;
    public float currentHealth;
    public float maxHealth;
    public int currentLevel;
    public float currentExp;
    public List<PlayerAbilityData> abilities;
    public List<SoulShard> ownedShards;
    public List<string> relicIds;
}

[Serializable]
public class PlayerAbilityData
{
    public string id;
    public int slots;
    public List<SoulShard> soulShards;
    public bool canBeModified;
}

public interface ILoadableEntity
{
    public void SetId(string id);

    public string GetId();
    public void LoadData(LoadableEntityData data);

    public LoadableEntityData GetData();

    public void Destroy();
}

[Serializable]
public class LoadableEntityData
{
    public string instanceId;
    public bool interactable;
    public bool wasInteracted;
    public string attachedId;
    public List<ObtainedEntity> obtainedEntities;
    public Vector3 position;
    public Quaternion rotation;
    public float health;
}

[Serializable]
public class LoadableEntityWrapper
{
    public List<LoadableEntityData> datas;
    public string campaignId;
    public int level;
}