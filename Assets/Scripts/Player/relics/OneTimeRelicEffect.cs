using UnityEngine;

public abstract class OneTimeRelicEffect : MonoBehaviour
{
    public abstract void ApplyEffect();

    public abstract void RelicDestroyed();

    public abstract string GetDescription();
}