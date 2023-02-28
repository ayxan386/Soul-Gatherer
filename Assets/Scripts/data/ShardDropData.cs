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
    private string changeVal;

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
                changeVal = shard.size.ToString("N1");
                break;
            case SoulShardType.Lifespan:
                shard.lifespan = Random.Range(floatRange.x, floatRange.y);
                changeVal = shard.lifespan.ToString("N1");
                break;
            case SoulShardType.Speed:
                shard.speed = Random.Range(floatRange.x, floatRange.y);
                changeVal = shard.speed.ToString("N1");
                break;
            case SoulShardType.ExplosiveRadius:
                shard.explosive = true;
                shard.explosionRadius = Random.Range(floatRange.x, floatRange.y);
                changeVal = shard.explosionRadius.ToString("N1");
                break;
            case SoulShardType.Vector:
                shard.force = Vector3.Lerp(vectorMin, vectorMax, Random.value);
                changeVal =
                    $"{shard.force.x:N1}, {shard.force.y:N1} , {shard.force.z:N1}";
                break;
        }

        shard.description += shard.effectRule == SoulShardEffectRule.Add ? " by " : " x";
        shard.description += changeVal;
    }
}