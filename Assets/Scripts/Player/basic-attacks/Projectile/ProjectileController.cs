using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileController : BaseParamAcceptingEntity
{
    private Rigidbody rb;
    private ProjectileParams details;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.velocity = transform.forward * details.speed;
        transform.localScale *= (details.radius / 0.5f);
        Destroy(gameObject, details.lifespan);
    }

    private void Update()
    {
        var colliders = Physics.OverlapSphere(transform.position, details.radius, details.collisionMask);
        if (colliders.Length > 0)
        {
            if (details.isExplosive)
            {
                var affectedEnt =
                    Physics.OverlapSphere(transform.position, details.explosionRadius, details.collisionMask);
                foreach (var entity in affectedEnt)
                {
                    if (entity.TryGetComponent(out Rigidbody rb))
                    {
                        rb.AddExplosionForce(details.explosionForce, transform.position, details.explosionRadius,
                            details.force.y);
                    }

                    // For enemies add damage here
                }
            }
            else
            {
                foreach (var collider in colliders)
                {
                    if (collider.TryGetComponent(out AbilityAffectedEntity entity))
                    {
                        entity.ApplyAbility(details);
                    }
                }
            }

            Destroy(gameObject);
        }
    }

    public override void ApplyParams(AbilityParam generalParam)
    {
        this.details = generalParam as ProjectileParams;
    }

    public override AbilityParam GetParams()
    {
        return details;
    }
}