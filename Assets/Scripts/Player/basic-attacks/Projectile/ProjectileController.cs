﻿using UnityEngine;

public class ProjectileController : BaseParamAcceptingEntity
{
    private ProjectileParams details;

    private float startSpeed;

    private void LateUpdate()
    {
        transform.Translate(transform.InverseTransformDirection(transform.forward) *
                            (startSpeed * Time.deltaTime));
        var colliders = Physics.OverlapSphere(transform.position, details.radius, details.collisionMask);
        if (colliders.Length > 0)
        {
            if (details.isExplosive)
            {
                var affectedEnt =
                    Physics.OverlapSphere(transform.position, details.explosionRadius, details.collisionMask);
                foreach (var entity in affectedEnt)
                {
                    CheckForAffectedEntityAndApply(entity);
                }
            }
            else
            {
                foreach (var collider in colliders)
                {
                    CheckForAffectedEntityAndApply(collider);
                }
            }

            Destroy(gameObject);
        }
    }

    private void CheckForAffectedEntityAndApply(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            EventStore.Instance.PublishPlayerAbilityAffected(GetParams());
            return;
        }

        IAbilityAffected entity;
        if (collider.TryGetComponent(out entity)
            || (collider.transform.parent != null && collider.transform.parent.TryGetComponent(out entity)))
        {
            entity.ApplyAbility(details);
        }
    }

    public override void ApplyParams(AbilityParam generalParam)
    {
        this.details = generalParam as ProjectileParams;
        transform.localScale *= (details.radius / 0.5f);
        startSpeed = details.speed + details.casterSpeed;
        details.casterSpeed = 0;
        Destroy(gameObject, details.lifespan);
    }

    public override AbilityParam GetParams()
    {
        return details;
    }
}