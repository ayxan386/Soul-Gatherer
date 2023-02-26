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