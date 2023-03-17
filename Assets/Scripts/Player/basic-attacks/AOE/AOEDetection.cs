using UnityEngine;
using UnityEngine.Events;

public class AOEDetection : MonoBehaviour
{
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private LayerMask aoeLayer;
    [SerializeField] private UnityEvent<AbilityParam> afterDetectionEvent;
    [SerializeField] private bool isAttachedToPlayer;
    private Vector3 pointA;
    private Vector3 pointB;

    private void Start()
    {
        pointA = capsuleCollider.center;
        pointA.y -= capsuleCollider.height / 2;
        pointB = capsuleCollider.center;
        pointB.y += capsuleCollider.height / 2;
    }

    void LateUpdate()
    {
        var overlapCapsule = Physics.OverlapCapsule(pointA + transform.position, pointB + transform.position,
            capsuleCollider.radius, aoeLayer);
        if (overlapCapsule.Length > 0)
        {
            foreach (var capsule in overlapCapsule)
            {
                if (capsule.TryGetComponent(out AOEAbilityController paramSource))
                {
                    var abilityParam = paramSource.GetParams();
                    if (isAttachedToPlayer && abilityParam.canAffectPlayer)
                    {
                        EventStore.Instance.PublishPlayerAbilityAffected(abilityParam);
                        return;
                    }

                    afterDetectionEvent?.Invoke(abilityParam);
                    break;
                }
            }
        }
    }
}