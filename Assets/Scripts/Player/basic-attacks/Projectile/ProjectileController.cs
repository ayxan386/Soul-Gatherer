using UnityEngine;

public class ProjectileController : BaseParamAcceptingEntity
{
    [SerializeField] private GameObject destructionParticles;
    private ProjectileParams details;

    private float startSpeed;
    private Vector3 particePosition;

    private void LateUpdate()
    {
        transform.Translate(transform.InverseTransformDirection(transform.forward) *
                            (startSpeed * Time.deltaTime));
        var colliders = Physics.OverlapSphere(transform.position, details.radius, details.collisionMask);
        if (colliders.Length <= 0) return;

        var shouldDestroy = false;
        particePosition = transform.position;
        if (details.isExplosive)
        {
            var affectedEnt =
                Physics.OverlapSphere(transform.position, details.explosionRadius, details.collisionMask);
            foreach (var entity in affectedEnt)
            {
                shouldDestroy = CheckForAffectedEntityAndApply(entity) || shouldDestroy;
            }
        }
        else
        {
            foreach (var collider in colliders)
            {
                shouldDestroy = CheckForAffectedEntityAndApply(collider) || shouldDestroy;
                if (shouldDestroy)
                {
                    particePosition = collider.ClosestPointOnBounds(transform.position);
                }
            }
        }

        if (shouldDestroy)
        {
            DeathParticles();
            Destroy(gameObject);
        }
    }

    private bool CheckForAffectedEntityAndApply(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            EventStore.Instance.PublishPlayerAbilityAffected(GetParams());
            return true;
        }

        IAbilityAffected entity;
        if (collider.TryGetComponent(out entity)
            || (collider.transform.parent != null && collider.transform.parent.TryGetComponent(out entity)))
        {
            entity.ApplyAbility(details);
            return entity.ShouldDestroyAbility(GetParams());
        }

        return false;
    }

    public override void ApplyParams(AbilityParam generalParam)
    {
        this.details = generalParam as ProjectileParams;
        transform.localScale *= (details.radius / 0.5f);
        startSpeed = details.speed + details.casterSpeed;
        details.casterSpeed = 0;
        Destroy(gameObject, details.lifespan);
        Invoke("DeathParticles", details.lifespan * 0.98f);
    }

    private void DeathParticles()
    {
        Instantiate(destructionParticles, particePosition, Quaternion.identity);
    }

    public override AbilityParam GetParams()
    {
        return details;
    }
}