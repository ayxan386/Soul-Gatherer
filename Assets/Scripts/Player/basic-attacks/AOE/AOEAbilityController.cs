using UnityEngine;

public class AOEAbilityController : BaseParamAcceptingEntity
{
    [SerializeField] private PolygonMeshGenerator meshGenerator;
    private AOEParams details;
    private MeshCollider coll;

    public override void ApplyParams(AbilityParam generalParam)
    {
        details = generalParam as AOEParams;
        meshGenerator.SetParams(details);
        meshGenerator.UpdateMesh();
        Destroy(gameObject, details.lifespan);
    }

    public override AbilityParam GetParams()
    {
        return details;
    }
}