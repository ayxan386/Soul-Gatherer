using System;
using UnityEngine;

public class AOEAbility : BaseAbility
{
    [SerializeField] private AOEParams details;

    public override void CastAbility(Transform centerPoint)
    {
        Instantiate(abilityPrefab, centerPoint.position, Quaternion.LookRotation(transform.forward))
            .ApplyParams(details);
    }
}

[Serializable]
public class AOEParams : AbilityParam
{
    public float thickness;
    public float height;
    [Range(0, 1f)] public float converageFraction;
    public float lifespan;
    public Vector3 force;
}