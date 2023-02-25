using UnityEngine;

public class AOEAbilityController : BaseAOEAbility
{
    [SerializeField] private PolygonMeshGenerator meshGenerator;
    private AOEParams details;
    private MeshCollider coll;

    public override void ApplyParams(AOEParams details)
    {
        this.details = details;
        meshGenerator.SetParams(this.details);
        meshGenerator.UpdateMesh();
        
        Destroy(gameObject, details.lifespan);
    }

    public AOEParams GetParams()
    {
        return details;
    }
}