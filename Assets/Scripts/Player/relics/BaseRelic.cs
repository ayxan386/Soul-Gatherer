using System.Collections.Generic;
using UnityEngine;

public class BaseRelic : MonoBehaviour
{
    [SerializeField] private new string name;
    [SerializeField] private bool canHaveMultiple;
    [SerializeField] private List<TickBasedRelicEffect> tickBasedRelicEffects;
    [SerializeField] private List<OneTimeRelicEffect> oneTimeRelicEffects;
    [SerializeField] private Sprite icon;
    [SerializeField] [Range(0, 1f)] private float dropChance;

    public Sprite Icon => icon;
    public bool CanHaveMultiple => canHaveMultiple;
    public string Name => name;

    public float DropChance => dropChance;

    public virtual void RelicObtained()
    {
        tickBasedRelicEffects.ForEach(tickBasedRelicEffect => tickBasedRelicEffect.StartEffect());
        oneTimeRelicEffects.ForEach(oneTimeRelicEffect => oneTimeRelicEffect.ApplyEffect());
    }

    public virtual void RelicDestroyed()
    {
        tickBasedRelicEffects.ForEach(tickBasedRelicEffect => tickBasedRelicEffect.RelicDestroyed());
        oneTimeRelicEffects.ForEach(oneTimeRelicEffect => oneTimeRelicEffect.RelicDestroyed());
    }

    public string GetDescription()
    {
        var res = name + "\n";
        foreach (var oneTimeRelicEffect in oneTimeRelicEffects)
        {
            res += oneTimeRelicEffect.GetDescription() + "\n";
        }

        foreach (var effect in tickBasedRelicEffects)
        {
            res += effect.GetDescription() + "\n";
        }

        return res;
    }
}