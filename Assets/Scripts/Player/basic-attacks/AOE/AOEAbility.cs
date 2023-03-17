using System;
using UnityEngine;

public class AOEAbility : BaseAbility
{
    [SerializeField] private AOEParams details;

    public override void CastAbility(Transform centerPoint)
    {
        Instantiate(abilityPrefab, centerPoint.position, Quaternion.LookRotation(transform.forward))
            .ApplyParams(details);
    }

    public override bool CanApplySoulShard(SoulShard soulShard)
    {
        return soulShard.type != SoulShardType.Explosive;
    }

    public override void ApplySoulShard(SoulShard soulShard)
    {
        switch (soulShard.type)
        {
            case SoulShardType.Size:
                ApplyToFloat(soulShard.value, soulShard.effectRule, ref details.thickness);
                break;
            case SoulShardType.Lifespan:
                ApplyToFloat(soulShard.value, soulShard.effectRule, ref details.lifespan);
                break;
            case SoulShardType.Vector:
                ApplyToVector(soulShard.force, ref details.force);
                break;
            case SoulShardType.Damage:
                ApplyToFloat(soulShard.value, soulShard.effectRule, ref details.damage);
                break;
        }

        base.ApplySoulShard(soulShard);
    }

    public override void RemoveSoulShard(SoulShard soulShard)
    {
        switch (soulShard.type)
        {
            case SoulShardType.Size:
                RemoveFromFloat(soulShard.value, soulShard.effectRule, ref details.thickness);
                break;
            case SoulShardType.Lifespan:
                RemoveFromFloat(soulShard.value, soulShard.effectRule, ref details.lifespan);
                break;
            case SoulShardType.Vector:
                RemoveFromVector(soulShard.force, ref details.force);
                break;
            case SoulShardType.Damage:
                RemoveFromFloat(soulShard.value, soulShard.effectRule, ref details.damage);
                break;
        }

        base.RemoveSoulShard(soulShard);
    }
}

[Serializable]
public class AOEParams : AbilityParam
{
    public float thickness;
    public float height;
    [Range(0, 1f)] public float converageFraction;
}