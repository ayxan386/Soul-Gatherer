using UnityEngine;

public class OneTimeSpeedBoostEffect : OneTimeRelicEffect
{
    [SerializeField] private float speedIncreasePercentage;

    public override void ApplyEffect()
    {
        EventStore.Instance.PublishPlayerMaxSpeedChange(speedIncreasePercentage);
    }

    public override void RelicDestroyed()
    {
        EventStore.Instance.PublishPlayerMaxSpeedChange(speedIncreasePercentage);
    }

    public override string GetDescription()
    {
        return $"Increase speed of player by {speedIncreasePercentage}%";
    }
}