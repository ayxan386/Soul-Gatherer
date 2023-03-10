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

    public delegate void AbilityPassingEvent(BaseAbility ability);

    public delegate void ShardPassingEvent(SoulShard soulShard);

}