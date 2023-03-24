using System;
using System.IO;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public const string fileName = "player-data.json";

    [ContextMenu("Save player data")]
    public void SaveData()
    {
        var playerWorldData = new PlayerWorldData();
        playerWorldData.position = PlayerMovement.Instance.transform.position;
        playerWorldData.rotation = PlayerMovement.Instance.transform.rotation;
        EventStore.Instance.PublishPlayerWorldDataPrepared(playerWorldData);
        var json = JsonUtility.ToJson(playerWorldData);
        print("Json to save: " + json);
        var path = Path.Combine(Application.dataPath, fileName);
        print("File path : " + path);
        File.WriteAllText(path, json);
    }
}

[Serializable]
public class PlayerWorldData
{
    public Vector3 position;
    public Quaternion rotation;
    public float currentHealth;
    public int currentLevel;
    public float currentExp;
}