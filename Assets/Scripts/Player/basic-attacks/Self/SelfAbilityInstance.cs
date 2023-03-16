using System;
using UnityEngine;

public class SelfAbilityInstance : BaseParamAcceptingEntity
{
    private SelfAbilityParams details;

    public override void ApplyParams(AbilityParam generalParam)
    {
        details = generalParam as SelfAbilityParams;

        if (details.force.magnitude > 0)
        {
            if (transform.parent.TryGetComponent(out IMoveableEntity me))
            {
                me.MoveBy(transform.parent.rotation * details.force);
            }
        }

        Destroy(gameObject, details.lifespan);
    }

    public override AbilityParam GetParams()
    {
        throw new NotImplementedException();
    }
}

public interface IMoveableEntity
{
    public void MoveBy(Vector3 force);
}