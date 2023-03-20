using System;
using UnityEngine;

public class EventStore : MonoBehaviour
{
    public static EventStore Instance;

    public event EventHandler<ObtainedEntity> OnEntityObtainedDisplay;

    public event EventHandler<ObtainedEntity> OnEntityObtainedClick;

    public event AbilityPassingEvent OnPlayerAbilityModify;
    public event Action<AbilityDisplayer> OnPlayerAbilityDisplayerClick;

    public event AbilityPassingEvent OnPlayerAbilityAdd;
    public event ShardPassingEvent OnShardAdd;
    public event ShardPassingEvent OnShardRemove;

    public event AbilityParamPassingEvent OnPlayerAbilityAffected;

    public event Action<float> OnPlayerHealthChange;

    public event Action<int> OnPlayerLevelUp;

    public event Action<BaseRelic> OnRelicObtained;

    public event Action<float, bool> OnPlayerHealingApplied;
    public event Action<float, bool> OnPlayerMaxHealthChange;
    public event Action OnPauseMenu;

    private void Awake()
    {
        Instance = this;
    }

    public void PublishEntityObtainedDisplay(ObtainedEntity obtainedEntity)
    {
        OnEntityObtainedDisplay?.Invoke(this, obtainedEntity);
    }

    public void PublishEntityObtainedClick(ObtainedEntity obtainedEntity)
    {
        OnEntityObtainedClick?.Invoke(this, obtainedEntity);
    }

    public void PublishPlayerAbilityModified(BaseAbility ability)
    {
        OnPlayerAbilityModify?.Invoke(ability);
    }

    public void PublishPlayerAbilityAffected(AbilityParam abilityParam)
    {
        OnPlayerAbilityAffected?.Invoke(abilityParam);
    }

    public void PublishPlayerAbilityDisplayerClick(AbilityDisplayer abilityDisplayer)
    {
        OnPlayerAbilityDisplayerClick?.Invoke(abilityDisplayer);
    }

    public void PublishPlayerAbilityAdd(BaseAbility ability)
    {
        OnPlayerAbilityAdd?.Invoke(ability);
    }

    public void PublishShardAdd(SoulShard shard)
    {
        OnShardAdd?.Invoke(shard);
    }

    public void PublishShardRemove(SoulShard shard)
    {
        OnShardRemove?.Invoke(shard);
    }

    public void PublishPlayerHealthChange(float currentHealth)
    {
        OnPlayerHealthChange?.Invoke(currentHealth);
    }

    public void PublishPlayerLevelUp(int currentLevel)
    {
        OnPlayerLevelUp?.Invoke(currentLevel);
    }

    public void PublishRelicObtained(BaseRelic relic)
    {
        OnRelicObtained?.Invoke(relic);
    }

    public void PublishPlayerHealingApplied(float amount, bool isFraction)
    {
        OnPlayerHealingApplied?.Invoke(amount, isFraction);
    }

    public void PublishPlayerMaxHealthChange(float amount, bool isFraction)
    {
        OnPlayerMaxHealthChange?.Invoke(amount, isFraction);
    }

    public void PublishPauseMenu()
    {
        OnPauseMenu?.Invoke();
    }

    public delegate void AbilityPassingEvent(BaseAbility ability);

    public delegate void AbilityParamPassingEvent(AbilityParam ability);

    public delegate void ShardPassingEvent(SoulShard soulShard);
}