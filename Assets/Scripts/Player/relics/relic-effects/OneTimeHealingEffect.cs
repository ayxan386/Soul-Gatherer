using UnityEngine;

public class OneTimeHealingEffect : OneTimeRelicEffect
{
    [SerializeField] private float healAmount;
    [SerializeField] private bool fraction;

    public override void ObtainedEffect()
    {
    }

    public override void UsedEffect()
    {
        EventStore.Instance.PublishPlayerHealingApplied(healAmount, fraction);
    }

    public override string GetDescription()
    {
        var amount = fraction ? healAmount * 100 : healAmount;
        var suffix = fraction ? "%" : "";
        return $"Heal player by {amount: 0}{suffix}";
    }
}