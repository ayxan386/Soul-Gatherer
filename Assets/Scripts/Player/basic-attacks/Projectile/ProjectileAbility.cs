using System;
using UnityEngine;

public class ProjectileAbility : BaseAbility
{
    [SerializeField] private ProjectileParams details;

    public override void CastAbility(Transform centerPoint)
    {
        details.casterSpeed = PlayerMovement.Instance.Speed;
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
                ApplyToFloat(soulShard.value, soulShard.effectRule, ref details.radius);
                break;
            case SoulShardType.Lifespan:
                ApplyToFloat(soulShard.value, soulShard.effectRule, ref details.lifespan);
                break;
            case SoulShardType.Vector:
                ApplyToVector(soulShard.force, ref details.force);
                break;
            case SoulShardType.Speed:
                ApplyToFloat(soulShard.value, soulShard.effectRule, ref details.speed);
                break;
            case SoulShardType.ExplosiveRadius:
                ApplyToFloat(soulShard.value, soulShard.effectRule, ref details.explosionRadius);
                details.isExplosive = true;
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
                RemoveFromFloat(soulShard.value, soulShard.effectRule, ref details.radius);
                break;
            case SoulShardType.Lifespan:
                RemoveFromFloat(soulShard.value, soulShard.effectRule, ref details.lifespan);
                break;
            case SoulShardType.Vector:
                RemoveFromVector(soulShard.force, ref details.force);
                break;
            case SoulShardType.Speed:
                RemoveFromFloat(soulShard.value, soulShard.effectRule, ref details.speed);
                break;
            case SoulShardType.ExplosiveRadius:
                RemoveFromFloat(soulShard.value, soulShard.effectRule, ref details.explosionRadius);
                details.isExplosive = false; // TODO Add check for other shards
                break;
            case SoulShardType.Damage:
                RemoveFromFloat(soulShard.value, soulShard.effectRule, ref details.damage);
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
    public bool isExplosive;
    public float explosionRadius;
    public float explosionForce;
    public LayerMask collisionMask;
    public float casterSpeed;
}