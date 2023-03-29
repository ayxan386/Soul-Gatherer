using System;
using UnityEngine;

public class AOEAbility : BaseAbility, IModifiableEntityAbility
{
    [SerializeField] private bool mobAttached;
    [SerializeField] private AOEParams details;

    public override void CastAbility(Transform centerPoint)
    {
        Instantiate(abilityPrefab, centerPoint.position, Quaternion.LookRotation(transform.forward))
            .ApplyParams(details);
    }

    public override bool CanApplySoulShard(SoulShard soulShard)
    {
        return soulShard.type != SoulShardType.ExplosiveRadius && soulShard.type != SoulShardType.Speed;
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

    public void IncreaseDamage(float mult)
    {
        if (mobAttached) details.damage *= mult;
    }

    public override string GetDescription()
    {
        var res = Desc + "\n";
        res += $"Damage : {details.damage:N1}\n";
        res += $"Lifespan : {details.lifespan:N1}\n";
        res += $"Thickness  : {details.thickness:N1} u\n";
        res += $"Height   : {details.height:N1} u\n";
        res += $"Force  : {details.force:N1}\n";
        res += $"Coverage: {details.converageFraction * 100:N1}%\n";
        return res;
    }
}

[Serializable]
public class AOEParams : AbilityParam
{
    public float thickness;
    public float height;
    [Range(0, 1f)] public float converageFraction;
}