using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableItem : MonoBehaviour, IAbilityAffected
{
    [SerializeField] private float hp;
    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private List<ItemInteractionBehavior> behaviors;

    private bool hurtEffects;
    private bool inProgress;
    private bool wasInteracted;

    [ContextMenu("Interact")]
    private void Interact()
    {
        print("Interacted");
        if (inProgress) return;
        inProgress = true;
        StartCoroutine(Interaction(new InteractionPassData(wasInteracted)));
    }

    private IEnumerator Interaction(InteractionPassData data)
    {
        foreach (var behavior in behaviors)
        {
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

    private void ApplyDeathEffects()
    {
        Interact();
    }

    void ResetAfterPeriod()
    {
        hurtEffects = false;
    }
}