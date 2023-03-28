using UnityEngine;

public abstract class OneTimeRelicEffect : MonoBehaviour
{
    public abstract void ObtainedEffect();

    public abstract void UsedEffect();

    public abstract string GetDescription();
}