using System;
using UnityEngine;

public class ProjectileAbility : BaseAbility
{
    [SerializeField] private ProjectileParams projectileParams;

    public override void CastAbility(Transform centerPoint)
    {
        Instantiate(abilityPrefab, centerPoint.position, Quaternion.LookRotation(centerPoint.forward))
            .ApplyParams(projectileParams);
    }
}

[Serializable]
public class ProjectileParams : AbilityParam
{
    public float speed;
    public float radius;
    public float lifespan;
    public Vector3 force;
    public bool isExplosive;
    public float explosionRadius;
    public float explosionForce;
    public LayerMask collisionMask;
}