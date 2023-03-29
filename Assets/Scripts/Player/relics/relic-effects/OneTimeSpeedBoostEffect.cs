using UnityEngine;

public class OneTimeSpeedBoostEffect : OneTimeRelicEffect
{
    [SerializeField] private float speedIncreasePercentage;

    public override void ObtainedEffect()
    {
        EventStore.Instance.PublishPlayerMaxSpeedChange(speedIncreasePercentage);
    }

    public override void UsedEffect()
    {
        EventStore.Instance.PublishPlayerMaxSpeedChange(-speedIncreasePercentage);
    }

    public override string GetDescription()
    {
        return $"Increase speed of player by {speedIncreasePercentage}%";
    }
}