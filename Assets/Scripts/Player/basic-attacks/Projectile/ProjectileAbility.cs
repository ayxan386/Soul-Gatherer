using System;
using UnityEngine;

public class ProjectileAbility : MonoBehaviour
{
    [SerializeField] private BaseProjectileAbility attackPrefab;
    [SerializeField] private float cooldown;
    [SerializeField] private ProjectileParams projectileParams;

    public void LaunchAttack(Transform attackPoint)
    {
        Instantiate(attackPrefab, attackPoint.position, Quaternion.LookRotation(attackPoint.forward))
            .ApplyParams(projectileParams);
    }

    public float GetCooldown()
    {
        return cooldown;
    }
}

[Serializable]
public class ProjectileParams
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

public abstract class BaseProjectileAbility : MonoBehaviour
{
    public abstract void ApplyParams(ProjectileParams details);
}