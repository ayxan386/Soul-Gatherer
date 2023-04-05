using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableItem : MonoBehaviour, IAbilityAffected, ILoadableEntity
{
    [SerializeField] private float hp;
    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private List<ItemInteractionBehavior> behaviors;

    private string attachedId;
    private bool hurtEffects;
    private bool inProgress;
    private bool wasInteracted;
    private ItemInteractionBehavior lastBehavior;
    private StringIdHolder assignedId;

    private void Awake()
    {
        assignedId = GetComponent<StringIdHolder>();
        print(assignedId);
    }

    [ContextMenu("Interact")]
    private void Interact()
    {
        print("Interacted");
        if (inProgress) return;
        inProgress = true;
        EventStore.Instance.OnItemInteractionCancelled += OnItemInteractionCancelled;
        StartCoroutine(Interaction(new InteractionPassData(wasInteracted)));
    }


    private IEnumerator Interaction(InteractionPassData data)
    {
        foreach (var behavior in behaviors)
        {
            lastBehavior = behavior;
            yield return new WaitForSeconds(behavior.DelayBefore);
            behavior.Interact(data);
            yield return new WaitUntil(() => behavior.Complete);
            yield return new WaitForSeconds(behavior.DelayAfter);
        }

        inProgress = false;
    }

    public void ApplyAbility(AbilityParam details)
    {
        TakeDamage(details.damage * (details.tickDamage ? Time.deltaTime : 1));
    }

    private void TakeDamage(float damage)
    {
        hp -= damage;
        if (damage > 0 && !hurtEffects)
        {
            hurtEffects = true;
            ItemInteraction.Instance.ItemInteractionSound?.PlayOneShot(damageSound);
            damageParticles.Play();
            Invoke("ResetAfterPeriod", 0.4f);
        }

        if (hp <= 0)
        {
            ApplyDeathEffects();
        }
    }

    private void OnItemInteractionCancelled(string id)
    {
        if (assignedId != null && id == assignedId.id)
        {
            inProgress = false;
            lastBehavior.Complete = true;
        }
    }

    private void ApplyDeathEffects()
    {
        Interact();
    }

    void ResetAfterPeriod()
    {
        hurtEffects = false;
    }

    public void LoadData(LoadableEntityData data)
    {
        wasInteracted = data.wasInteracted;
    }

    public LoadableEntityData GetData()
    {
        var res = new LoadableEntityData();
        res.wasInteracted = wasInteracted;
        res.instanceId = GetId();
        return res;
    }

    public void SetId(string id)
    {
        print("Set id called");
        assignedId = gameObject.AddComponent<StringIdHolder>();
        assignedId.id = id;
    }

    public string GetId()
    {
        return assignedId.id + GetType().Name;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}