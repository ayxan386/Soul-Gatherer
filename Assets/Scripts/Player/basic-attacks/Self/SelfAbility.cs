using System;
using UnityEngine;

public class SelfAbility : BaseAbility
{
    [SerializeField] private SelfAbilityParams details;

    public override void CastAbility(Transform centerPoint)
    {
        Instantiate(abilityPrefab, centerPoint.position, Quaternion.identity, centerPoint)
            .ApplyParams(details);
    }

    public override bool CanApplySoulShard(SoulShard soulShard)
    {
        return soulShard.type is SoulShardType.Lifespan or SoulShardType.Vector;
    }

    public override void ApplySoulShard(SoulShard soulShard)
    {
        switch (soulShard.type)
        {
            case SoulShardType.Lifespan:
                ApplyToFloat(soulShard.value, soulShard.effectRule, ref details.lifespan);
                break;
            case SoulShardType.Vector:
                ApplyToVector(soulShard.force, ref details.force);
                break;
        }

        base.ApplySoulShard(soulShard);
    }

    public override void RemoveSoulShard(SoulShard soulShard)
    {
        switch (soulShard.type)
        {
            case SoulShardType.Lifespan:
                RemoveFromFloat(soulShard.value, soulShard.effectRule, ref details.lifespan);
                break;
            case SoulShardType.Vector:
                RemoveFromVector(soulShard.force, ref details.force);
                break;
        }

        base.RemoveSoulShard(soulShard);
    }
}

[Serializable]
public class SelfAbilityParams : AbilityParam
{
}