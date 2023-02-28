using System;
using UnityEngine;

public class ProjectileAbility : BaseAbility
{
    [SerializeField] private ProjectileParams details;

    public override void CastAbility(Transform centerPoint)
    {
        Instantiate(abilityPrefab, centerPoint.position, Quaternion.LookRotation(centerPoint.forward))
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
                ApplyToFloat(soulShard.size, soulShard.effectRule, ref details.radius);
                break;
            case SoulShardType.Lifespan:
                ApplyToFloat(soulShard.lifespan, soulShard.effectRule, ref details.lifespan);
                break;
            case SoulShardType.Vector:
                ApplyToVector(soulShard.force, ref details.force);
                break;
            case SoulShardType.Speed:
                ApplyToFloat(soulShard.speed, soulShard.effectRule, ref details.speed);
                break;
            case SoulShardType.ExplosiveRadius:
                ApplyToFloat(soulShard.explosionRadius, soulShard.effectRule, ref details.explosionRadius);
                details.isExplosive = true;
                break;
        }

        base.ApplySoulShard(soulShard);
    }

    public override void RemoveSoulShard(SoulShard soulShard)
    {
        switch (soulShard.type)
        {
            case SoulShardType.Size:
                RemoveFromFloat(soulShard.size, soulShard.effectRule, ref details.radius);
                break;
            case SoulShardType.Lifespan:
                RemoveFromFloat(soulShard.lifespan, soulShard.effectRule, ref details.lifespan);
                break;
            case SoulShardType.Vector:
                RemoveFromVector(soulShard.force, ref details.force);
                break;
            case SoulShardType.Speed:
                RemoveFromFloat(soulShard.speed, soulShard.effectRule, ref details.speed);
                break;
            case SoulShardType.ExplosiveRadius:
                RemoveFromFloat(soulShard.explosionRadius, soulShard.effectRule, ref details.explosionRadius);
                details.isExplosive = false;
                break;
        }

        base.RemoveSoulShard(soulShard);
    }
}

[Serializable]
public class ProjectileParams : AbilityParam
{
    public float speed;
    public float radius;
    public float lifespan;
    public bool isExplosive;
    public float explosionRadius;
    public float explosionForce;
    public LayerMask collisionMask;
}