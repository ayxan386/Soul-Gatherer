using System;
using UnityEngine;

public class EventStore : MonoBehaviour
{
    public static EventStore Instance;

    public event EventHandler<ObtainedEntity> OnEntityObtainedDisplay;

    public event EventHandler<ObtainedEntity> OnEntityObtainedClick;

    public event AbilityPassingEvent OnPlayerAbilityModify;
    public event AbilityPassingEvent OnPlayerAbilityDisplayerClick;

    public event AbilityPassingEvent OnPlayerAbilityAdd;
    public event ShardPassingEvent OnShardAdd;
    public event ShardPassingEvent OnShardRemove;

    public event AbilityParamPassingEvent OnPlayerAbilityAffected;

    public event Action<float> OnPlayerHealthChange;

    public event Action<int> OnPlayerLevelUp;

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

    public void PublishPlayerAbilityDisplayerClick(BaseAbility ability)
    {
        OnPlayerAbilityDisplayerClick?.Invoke(ability);
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

    public delegate void AbilityPassingEvent(BaseAbility ability);

    public delegate void AbilityParamPassingEvent(AbilityParam ability);

    public delegate void ShardPassingEvent(SoulShard soulShard);
}