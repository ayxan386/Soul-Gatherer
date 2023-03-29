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
    [SerializeField] private bool canBeModified;

    [Header("For UI display")] [SerializeField]
    protected Sprite icon;

    [SerializeField] protected string description;

    public Sprite Icon => icon;
    public string Desc => description;
    public string Id => id;

    public bool CanBeModified => canBeModified;

    public int AvailableSlots => currentNumberOfSlots;
    public List<SoulShard> InstalledShards => soulShards;

    public abstract void CastAbility(Transform centerPoint);

    public float GetCooldown()
    {
        return cooldown;
    }

    public void ExpandSlotCount()
    {
        currentNumberOfSlots++;
    }

    public abstract bool CanApplySoulShard(SoulShard soulShard);

    public virtual void ApplySoulShard(SoulShard soulShard)
    {
        if (soulShards.Count + 1 > currentNumberOfSlots) return;
        soulShards.Add(soulShard);
        print("Attaching soul shard: " + soulShard);
        soulShard.attached = true;
        soulShard.abilityId = Id;
        EventStore.Instance.PublishPlayerAbilityModified(this);
    }

    public virtual void RemoveSoulShard(SoulShard soulShard)
    {
        if (soulShards.Count == 0) return;
        print("BaseAbility.remove before : " + soulShard);
        soulShards.Remove(soulShard);
        soulShard.attached = false;
        soulShard.abilityId = "not attached";
        print("BaseAbility.remove after: " + soulShard);
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

    protected void ApplyToVector(Vector3 increament, ref Vector3 par)
    {
        par += increament;
    }

    protected void RemoveFromVector(Vector3 decrement, ref Vector3 par)
    {
        par -= decrement;
    }

    public PlayerAbilityData GetData()
    {
        var res = new PlayerAbilityData();
        res.id = id;
        res.soulShards = soulShards;
        res.canBeModified = canBeModified;
        res.slots = currentNumberOfSlots;
        return res;
    }

    public void ApplyData(PlayerAbilityData data)
    {
        id = data.id;
        canBeModified = data.canBeModified;
        currentNumberOfSlots = data.slots;
    }

    public virtual string GetDescription()
    {
        var res = Desc + "\n";
        return res;
    }
}


[Serializable]
public class AbilityParam
{
    public float damage;
    public bool tickDamage;
    public float lifespan;
    public Vector3 force;
    public bool canAffectPlayer;
}

public abstract class BaseParamAcceptingEntity : MonoBehaviour
{
    public abstract void ApplyParams(AbilityParam generalParam);

    public abstract AbilityParam GetParams();
}

[Serializable]
public class SoulShard
{
    public string id;
    public string abilityId;
    public Sprite icon;
    public SoulShardType type;
    public SoulShardEffectRule effectRule;
    public bool attached;
    public string description;
    public Vector3 force;
    public float value;
    public bool explosive;

    public SoulShard()
    {
        id = Guid.NewGuid().ToString();
    }

    public SoulShard(SoulShard source)
    {
        id = source.id;
        abilityId = source.abilityId;
        icon = source.icon;
        type = source.type;
        effectRule = source.effectRule;
        attached = source.attached;
        description = source.description;
        force = source.force;
        value = source.value;
        explosive = source.explosive;
    }

    public override string ToString()
    {
        return $"{type.ToString()} x {value} x {attached} => {abilityId}";
    }

    public override bool Equals(object obj)
    {
        if (obj is not SoulShard) return false;
        var casted = obj as SoulShard;
        return casted.id == id;
    }
}

public enum SoulShardType
{
    Vector,
    Size,
    Lifespan,
    Speed,
    ExplosiveRadius,
    Damage,
    Explosive,
}

public enum SoulShardEffectRule
{
    Add,
    Multiply
}

public interface IAbilityAffected
{
    public void ApplyAbility(AbilityParam details);
}