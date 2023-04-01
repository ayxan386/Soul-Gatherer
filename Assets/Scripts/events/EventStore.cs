using System;
using UnityEngine;

public class EventStore : MonoBehaviour
{
    public static EventStore Instance { get; private set; }

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
    public event Action<BaseRelic> OnRelicDestroyed;

    public event Action<float, bool> OnPlayerHealingApplied;
    public event Action<float, bool> OnPlayerMaxHealthChange;

    public event Action<float> OnPlayerMaxSpeedChange;
    
    public event Action<float> OnExpBoostFraction;
    public event Action OnPauseMenu;

    public event Action OnRelicInventoryUpdate;

    public event Action<PlayerWorldData> OnPlayerDataSave;

    public event Action<PlayerWorldData> OnPlayerDataLoad;

    private void Awake()
    {
        print("Event store awake");
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

    public void PublishPlayerMaxSpeedChange(float changePercentage)
    {
        OnPlayerMaxSpeedChange?.Invoke(changePercentage);
    }

    public void PublishPlayerWorldDataPrepared(PlayerWorldData data)
    {
        OnPlayerDataSave?.Invoke(data);
    }

    public void PublishPlayerWorldDataLoaded(PlayerWorldData playerWorldData)
    {
        OnPlayerDataLoad?.Invoke(playerWorldData);
    }

    public void PublishRelicDestroyed(BaseRelic destroyedRelic)
    {
        OnRelicDestroyed?.Invoke(destroyedRelic);
    }

    public void PublishRelicInventoryUpdated()
    {
        OnRelicInventoryUpdate?.Invoke();
    }

    public void PublishExpBoostFraction(float experienceBoostFraction)
    {
        OnExpBoostFraction?.Invoke(experienceBoostFraction);
    }

    public delegate void AbilityPassingEvent(BaseAbility ability);

    public delegate void AbilityParamPassingEvent(AbilityParam ability);

    public delegate void ShardPassingEvent(SoulShard soulShard);

    public delegate float FloatPassingEvent();
}