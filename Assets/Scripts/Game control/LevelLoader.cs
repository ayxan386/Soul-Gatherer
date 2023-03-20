using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelLoader : MonoBehaviour
{
    private const string CampaignKey = "campaign";
    private const string CurrentLevel = "CurrentLevel";
    [SerializeField] private List<string> levelNames;
    [SerializeField] private int length;
    public LevelLoader Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GenerateCampaign()
    {
        Random.InitState((Time.deltaTime * 10000).ToString().GetHashCode());
        var campaignDatas = new List<CampaignData>(length);
        for (int i = 0; i < length; i++)
        {
            var index = Random.Range(0, levelNames.Count);
            print("random index: " + index);
            var campaignData = new CampaignData();
            campaignData.levelName = levelNames[index];
            campaignData.difficulty = i + 1;
            campaignData.isLast = false;
            campaignDatas.Add(campaignData);
        }

        campaignDatas[length - 1].isLast = true;

        var campaignDataWrapper = new CampaignDataWrapper();
        campaignDataWrapper.campaignDatas = campaignDatas;
        var json = JsonUtility.ToJson(campaignDataWrapper);
        PlayerPrefs.SetString(CampaignKey, json);
        PlayerPrefs.SetInt(CurrentLevel, 0);
        PlayerPrefs.Save();
    }

    public CampaignData GetCurrentLevel()
    {
        return JsonUtility.FromJson<CampaignDataWrapper>(PlayerPrefs.GetString(CampaignKey))
            .campaignDatas[PlayerPrefs.GetInt(CurrentLevel)];
    }

    [ContextMenu("Debug data")]
    public void DebugCampaignData()
    {
        var hasActive = HasActiveCampaign();
        print("Has active campaign : " + hasActive);
        if (hasActive)
        {
            print("Whole campaign : " + PlayerPrefs.GetString(CampaignKey));
            print("Current campaign:" + GetCurrentLevel().levelName);
        }
    }

    public bool HasActiveCampaign()
    {
        return PlayerPrefs.HasKey(CurrentLevel) && PlayerPrefs.GetInt(CurrentLevel) >= 0;
    }

    [ContextMenu("Delete campaign data")]
    public void AbandonRun()
    {
        PlayerPrefs.DeleteKey(CurrentLevel);
        PlayerPrefs.DeleteKey(CampaignKey);
    }
}

[Serializable]
public class CampaignData
{
    public string levelName;
    public float difficulty;
    public bool isLast;
}

[Serializable]
public class CampaignDataWrapper
{
    public List<CampaignData> campaignDatas;
}