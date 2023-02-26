using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AbilityAffectedEntity : MonoBehaviour
{
    [SerializeField] private LayerMask abilityCheckLayer;
    [SerializeField] private bool applyForce = true;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundMaxDistance = 0.01f;
    [SerializeField] private bool takeFallDamage;
    [SerializeField] private float fallDamageConversionFactor;
    private Rigidbody rb;
    private NavMeshAgent agent;

    void Start()
    {
        TryGetComponent(out rb);
        TryGetComponent(out agent);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(groundCheckPoint.position, Vector3.down * groundMaxDistance);
    }

    public void ApplyAbility(AbilityParam ability)
    {
        print("Applying damage to: " + ability.damage);
        if (applyForce && ability.force.magnitude > 0)
        {
            if (agent != null)
            {
                StartCoroutine(ApplyForceToAgent(ability));
            }
            else if (rb != null)
                rb.AddForce(ability.force, ForceMode.Impulse);
        }
    }

    private IEnumerator ApplyForceToAgent(AbilityParam ability)
    {
        agent.enabled = false;
        rb.AddForce(ability.force, ForceMode.Impulse);
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitUntil(() =>
            Physics.Raycast(groundCheckPoint.position, Vector3.down, groundMaxDistance, groundLayer));
        if (takeFallDamage)
        {
            var fallDamage = rb.velocity.y * fallDamageConversionFactor;
            print("Took " + fallDamage + " fall damage");
        }
        agent.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((1 << collision.gameObject.layer & abilityCheckLayer) == 1 &&
            collision.collider.TryGetComponent(out BaseParamAcceptingEntity abilityController))
        {
            ApplyAbility(abilityController.GetParams());
        }
    }
}