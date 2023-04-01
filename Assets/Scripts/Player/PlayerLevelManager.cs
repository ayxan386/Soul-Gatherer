using TMPro;
using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
    [SerializeField] private AnimationCurve expRequired;
    [SerializeField] private float curveCorrectionFactor;
    [SerializeField] private int allowedMaxLevel;
    [SerializeField] private TextMeshProUGUI currentLevelText;

    private float currentExp;
    private int currentLevel;
    private float expFactor = 1;

    private void Start()
    {
        EventStore.Instance.OnEntityObtainedClick += OnEntityObtained;
        EventStore.Instance.OnPlayerDataSave += OnPlayerDataSave;
        EventStore.Instance.OnPlayerDataLoad += OnPlayerDataLoad;
        EventStore.Instance.OnExpBoostFraction += OnExpBoostFraction;
        AddExp(0);
    }

    private void OnExpBoostFraction(float fractionalIncrement)
    {
        expFactor += fractionalIncrement;
    }

    private void OnDestroy()
    {
        EventStore.Instance.OnEntityObtainedClick -= OnEntityObtained;
        EventStore.Instance.OnPlayerDataSave -= OnPlayerDataSave;
        EventStore.Instance.OnPlayerDataLoad -= OnPlayerDataLoad;
        EventStore.Instance.OnExpBoostFraction -= OnExpBoostFraction;
    }

    private void OnPlayerDataLoad(PlayerWorldData obj)
    {
        currentLevel = obj.currentLevel;
        currentExp = obj.currentExp;
        UpdateUi();
    }

    private void OnPlayerDataSave(PlayerWorldData obj)
    {
        obj.currentLevel = currentLevel;
        obj.currentExp = currentExp;
    }

    private void OnEntityObtained(object sender, ObtainedEntity ent)
    {
        if (ent.data.type == EntityType.Exp)
        {
            AddExp(ent.count);
        }
    }

    private void AddExp(int expIncrement)
    {
        currentExp += expFactor * expIncrement;
        while (currentExp >= GetRequiredExp())
        {
            currentExp -= GetRequiredExp();
            currentLevel++;
            EventStore.Instance.PublishPlayerLevelUp(currentLevel);
        }

        UpdateUi();
    }

    private void UpdateUi()
    {
        var progress = currentExp / GetRequiredExp();
        currentLevelText.text = $"LVL: {currentLevel} ({progress * 100:0}%)";
    }


    private float GetRequiredExp()
    {
        var x = 1f * currentLevel / allowedMaxLevel;
        return Mathf.RoundToInt(expRequired.Evaluate(x) * curveCorrectionFactor);
    }
}