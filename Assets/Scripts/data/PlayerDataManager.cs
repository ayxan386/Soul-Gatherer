using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public const string fileName = "player-data.json";

    [ContextMenu("Save player data")]
    public void SaveData()
    {
        var playerWorldData = new PlayerWorldData();
        EventStore.Instance.PublishPlayerWorldDataPrepared(playerWorldData);
        var json = JsonUtility.ToJson(playerWorldData);
        var path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(path, json);
    }

    [ContextMenu("Load player data")]
    public void LoadData()
    {
        var path = Path.Combine(Application.persistentDataPath, fileName);
        var json = File.ReadAllText(path);
        var playerWorldData = JsonUtility.FromJson<PlayerWorldData>(json);
        EventStore.Instance.PublishPlayerWorldDataLoaded(playerWorldData);
    }
}

[Serializable]
public class PlayerWorldData
{
    public Vector3 position;
    public Quaternion rotation;
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