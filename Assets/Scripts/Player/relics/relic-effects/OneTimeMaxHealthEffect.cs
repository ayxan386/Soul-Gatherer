using System;
using UnityEngine;

public class OneTimeMaxHealthEffect : OneTimeRelicEffect
{
    [SerializeField] private float value;
    [SerializeField] private bool fraction;

    public override void ObtainedEffect()
    {
        EventStore.Instance.PublishPlayerMaxHealthChange(value, fraction);
    }

    public override void UsedEffect()
    {
    }

    public override string GetDescription()
    {
        var amount = fraction ? value * 100 : value;
        var suffix = fraction ? "%" : "";
        var dir = value > 0 ? "Increase" : "Decrease";
        return $"{dir} player max health by {amount: 0}{suffix}";
    }
}