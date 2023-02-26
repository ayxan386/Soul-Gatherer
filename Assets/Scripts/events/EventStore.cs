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

    public delegate void AbilityPassingEvent(BaseAbility ability);
}