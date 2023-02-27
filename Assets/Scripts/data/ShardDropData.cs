using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ShardDrop", order = 1)]
public class ShardDropData : ScriptableObject
{
    public Sprite icon;
    public SoulShardType type;
    public bool effectRuleRandom;
    public SoulShardEffectRule effectRule;
    public Vector2 floatRange;
    public Vector3 vectorMin;
    public Vector3 vectorMax;
    public string desc;

    public void ApplyTo(SoulShard shard)
    {
        shard.description = desc;
        shard.icon = icon;
        shard.type = type;
        if (effectRuleRandom)
        {
            shard.effectRule = Random.value > 0.5f ? SoulShardEffectRule.Add : SoulShardEffectRule.Multiply;
        }
        else
        {
            shard.effectRule = effectRule;
        }

        switch (type)
        {
            case SoulShardType.Size:
                shard.size = Random.Range(floatRange.x, floatRange.y);
                break;
            case SoulShardType.Lifespan:
                shard.lifespan = Random.Range(floatRange.x, floatRange.y);
                break;
            case SoulShardType.Speed:
                shard.speed = Random.Range(floatRange.x, floatRange.y);
                break;
            case SoulShardType.ExplosiveRadius:
                shard.explosive = true;
                shard.explosionRadius = Random.Range(floatRange.x, floatRange.y);
                break;
            case SoulShardType.Vector:
                shard.force = Vector3.Lerp(vectorMin, vectorMax, Random.value);
                break;
        }
    }
}