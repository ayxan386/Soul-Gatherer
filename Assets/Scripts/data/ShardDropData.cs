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
            case SoulShardType.Vector:
                shard.force = Vector3.Lerp(vectorMin, vectorMax, Random.value);
                changeVal =
                    $"{shard.force.x:N1}, {shard.force.y:N1} , {shard.force.z:N1}";
                break;
            default:
                shard.value = Random.Range(floatRange.x, floatRange.y);
                shard.explosive = type == SoulShardType.Explosive;
                changeVal = shard.value.ToString("N1");
                break;
        }

        shard.description += shard.effectRule == SoulShardEffectRule.Add ? " by " : " x";
        shard.description += changeVal;
    }
}