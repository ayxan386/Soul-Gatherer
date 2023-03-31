using UnityEngine;

public class AOEAbilityController : BaseParamAcceptingEntity, IAbilityAffected
{
    [SerializeField] private PolygonMeshGenerator meshGenerator;
    private AOEParams details;
    private MeshCollider coll;
    private float health;

    public override void ApplyParams(AbilityParam generalParam)
    {
        details = generalParam as AOEParams;
        meshGenerator.SetParams(details);
        meshGenerator.UpdateMesh();
        if (details.isSolid)
        {
            health = details.solidHealth;
        }

        Destroy(gameObject, details.lifespan);
    }

    public override AbilityParam GetParams()
    {
        return details;
    }

    public void ApplyAbility(AbilityParam passedDetails)
    {
        if (!details.isSolid) return;
        if (passedDetails is ProjectileParams)
        {
            health -= passedDetails.damage;
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public bool ShouldDestroyAbility(AbilityParam passedParam)
    {
        return details.isSolid && passedParam is not AOEParams;
    }
}