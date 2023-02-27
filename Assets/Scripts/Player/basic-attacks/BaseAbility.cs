using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbility : MonoBehaviour
{
    [SerializeField] protected string id;
    [SerializeField] protected float cooldown;
    [SerializeField] protected BaseParamAcceptingEntity abilityPrefab;
    [SerializeField] protected int currentNumberOfSlots;
    [SerializeField] protected List<SoulShard> soulShards;

    [Header("For UI display")] [SerializeField]
    protected Sprite icon;

    [SerializeField] protected string description;

    public Sprite Icon => icon;
    public string Desc => description;
    public string Id => id;

    public int AvailableSlots => currentNumberOfSlots;
    public List<SoulShard> InstalledShards => soulShards;

    public abstract void CastAbility(Transform centerPoint);

    public float GetCooldown()
    {
        return cooldown;
    }

    public abstract bool CanApplySoulShard(SoulShard soulShard);

    public virtual void ApplySoulShard(SoulShard soulShard)
    {
        if (soulShards.Count + 1 > currentNumberOfSlots) return;
        soulShards.Add(soulShard);
        soulShard.attachedAbility = this;
        EventStore.Instance.PublishPlayerAbilityModified(this);
    }

    public virtual void RemoveSoulShard(SoulShard soulShard)
    {
        if (soulShards.Count == 0) return;
        soulShards.Remove(soulShard);
        soulShard.attachedAbility = null;
        EventStore.Instance.PublishPlayerAbilityModified(this);
    }


    protected void RemoveFromFloat(float decrement, SoulShardEffectRule effectRule, ref float par)
    {
        if (effectRule == SoulShardEffectRule.Add)
        {
            par -= decrement;
        }
        else
        {
            par /= decrement;
        }
    }

    protected void ApplyToFloat(float increment, SoulShardEffectRule effectRule, ref float par)
    {
        if (effectRule == SoulShardEffectRule.Add)
        {
            par += increment;
        }
        else
        {
            par *= increment;
        }
    }
}


public class AbilityParam
{
    public float damage;
    public Vector3 force;
}

public abstract class BaseParamAcceptingEntity : MonoBehaviour
{
    public abstract void ApplyParams(AbilityParam generalParam);

    public abstract AbilityParam GetParams();
}

[Serializable]
public class SoulShard
{
    public BaseAbility attachedAbility;
    public Sprite icon;
    public SoulShardType type;
    public SoulShardEffectRule effectRule;
    public string description;
    public Vector3 force;
    public float size;
    public float lifespan;
    public float speed;
    public bool explosive;
    public float explosionRadius;
}

public enum SoulShardType
{
    Vector,
    Size,
    Lifespan,
    Speed,
    ExplosiveRadius,
    Explosive,
}

public enum SoulShardEffectRule
{
    Add,
    Multiply
}