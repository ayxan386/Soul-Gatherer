using System;
using UnityEngine;

public class AOEAbility : MonoBehaviour
{
    [SerializeField] private BaseAOEAbility abilityPrefab;
    [SerializeField] private AOEParams details;
    [SerializeField] private float cooldown;

    public void CastAbility(Transform centerPoint)
    {
        Instantiate(abilityPrefab, centerPoint.position, Quaternion.identity)
            .ApplyParams(details);
    }

    public float GetCooldown()
    {
        return cooldown;
    }
}

[Serializable]
public class AOEParams
{
    public float thickness;
    public float height;
    [Range(0,1f)]
    public float converageFraction;
    public float lifespan;
    public Vector3 force;
}

public abstract class BaseAOEAbility : MonoBehaviour
{
    public abstract void ApplyParams(AOEParams details);
}