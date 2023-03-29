using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AbilityAffectedEntity : MonoBehaviour, IAbilityAffected, ILoadableEntity
{
    [SerializeField] private LayerMask abilityCheckLayer;

    [Header("Force&Gravity params")] [SerializeField]
    private bool applyForce = true;

    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundMaxDistance = 0.01f;

    [Header("Fall damage")] [SerializeField]
    private bool takeFallDamage;

    [SerializeField] private float fallDamageConversionFactor;
    [Header("Health")] [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private bool xScaleBasedHealthDisplay;
    [SerializeField] private Transform scaleBasedHealth;
    [SerializeField] private AudioSource effectSource;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private ParticleSystem bloodParticles;
    [Header("Death")] [SerializeField] private InteractableItem afterDeathInteraction;
    [SerializeField] private Behaviour[] toDisableOnDeath;
    private StringIdHolder assignedId;

    private Rigidbody rb;
    private NavMeshAgent agent;
    private bool hurtEffects;

    private void Awake()
    {
        assignedId = GetComponent<StringIdHolder>();
    }

    void Start()
    {
        afterDeathInteraction.Interactable = false;
        currentHealth = maxHealth * (LevelLoader.Instance ? LevelLoader.Instance.CalculateDifficulty() : 1);
        TryGetComponent(out rb);
        if (TryGetComponent(out agent))
        {
            rb.isKinematic = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(groundCheckPoint.position, Vector3.down * groundMaxDistance);
    }

    public void ApplyAbility(AbilityParam ability)
    {
        TakeDamage(ability.damage * (ability.tickDamage ? Time.deltaTime : 1));

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
        rb.isKinematic = false;
        rb.AddForce(ability.force, ForceMode.Impulse);
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitUntil(() =>
            Physics.Raycast(groundCheckPoint.position, Vector3.down, groundMaxDistance, groundLayer));
        if (takeFallDamage)
        {
            var fallDamage = rb.velocity.y * fallDamageConversionFactor;
            print("Took " + fallDamage + " fall damage");
            TakeDamage(-fallDamage);
        }

        rb.isKinematic = true;
        agent.enabled = currentHealth > 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((1 << collision.gameObject.layer & abilityCheckLayer) == 1 &&
            collision.collider.TryGetComponent(out BaseParamAcceptingEntity abilityController))
        {
            ApplyAbility(abilityController.GetParams());
        }
    }

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth > 0 && damage > 0 && !hurtEffects)
        {
            hurtEffects = true;
            effectSource?.PlayOneShot(damageSound);
            bloodParticles.Play();
            Invoke("ResetAfterPeriod", 0.4f);
        }

        if (xScaleBasedHealthDisplay)
        {
            scaleBasedHealth.localScale = new Vector3(Mathf.Clamp01(currentHealth / maxHealth), 1, 1);
        }

        if (currentHealth <= 0)
        {
            ApplyDeathEffects();
        }
    }

    private void ApplyDeathEffects()
    {
        agent.enabled = false;
        foreach (var comp in toDisableOnDeath)
        {
            comp.enabled = false;
        }

        afterDeathInteraction.Interactable = true;
    }

    void ResetAfterPeriod()
    {
        hurtEffects = false;
    }

    public void SetId(string id)
    {
        assignedId = gameObject.AddComponent<StringIdHolder>();
        assignedId.id = id;
    }

    public string GetId()
    {
        return assignedId.id + GetType().Name;
    }

    public void LoadData(LoadableEntityData data)
    {
        agent.enabled = false;
        transform.localPosition = data.position;
        transform.localRotation = data.rotation;
        currentHealth = data.health;
        TakeDamage(0);
        agent.enabled = true;
    }

    public LoadableEntityData GetData()
    {
        var entityData = new LoadableEntityData();
        entityData.instanceId = GetId();
        entityData.position = transform.localPosition;
        entityData.rotation = transform.localRotation;
        entityData.health = currentHealth;
        return entityData;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}