using System.Collections;
using UnityEngine;

public abstract class TickBasedRelicEffect : MonoBehaviour
{
    [SerializeField] private float period;
    private Coroutine periodicEffect;
    public float Period => period;

    public void StartEffect()
    {
        periodicEffect = StartCoroutine(PeriodicEffect());
    }

    private IEnumerator PeriodicEffect()
    {
        while (true)
        {
            TickEffect();
            yield return new WaitForSeconds(period);
        }
    }

    public abstract void TickEffect();

    public virtual void RelicDestroyed()
    {
        StopCoroutine(periodicEffect);
    }

    public abstract string GetDescription();
}