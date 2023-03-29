using UnityEngine;

public class RelicSelfDestructEffect : OneTimeRelicEffect
{
    [SerializeField] private GameObject target;
    [SerializeField] private BaseRelic destroyedRelic;

    public override void ObtainedEffect()
    {
    }

    public override void UsedEffect()
    {
        EventStore.Instance.PublishRelicDestroyed(destroyedRelic);
        Destroy(target);
    }

    public override string GetDescription()
    {
        return "";
    }
}