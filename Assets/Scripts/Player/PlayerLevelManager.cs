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

    private void Start()
    {
        EventStore.Instance.OnEntityObtainedClick += OnEntityObtained;
        AddExp(0);
    }

    private void OnDestroy()
    {
        EventStore.Instance.OnEntityObtainedClick -= OnEntityObtained;
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
        currentExp += expIncrement;
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