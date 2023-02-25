using UnityEngine;

public abstract class BaseAbility : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    [SerializeField] protected BaseParamAcceptingEntity abilityPrefab;

    public abstract void CastAbility(Transform centerPoint);

    public float GetCooldown()
    {
        return cooldown;
    }
}

public interface AbilityParam
{
}

public abstract class BaseParamAcceptingEntity : MonoBehaviour
{
    public abstract void ApplyParams(AbilityParam generalParam);

    public abstract AbilityParam GetParams();
}