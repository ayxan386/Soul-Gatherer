using System.Collections.Generic;
using UnityEngine;

public class BaseRelic : MonoBehaviour
{
    [SerializeField] private new string name;
    [SerializeField] private bool canHaveMultiple;
    [SerializeField] private List<TickBasedRelicEffect> tickBasedRelicEffects;
    [SerializeField] private List<OneTimeRelicEffect> oneTimeRelicEffects;

    public bool CanHaveMultiple => canHaveMultiple;
    public string Name => name;

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
}