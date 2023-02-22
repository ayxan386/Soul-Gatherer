using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    [SerializeField] protected List<ItemInteractionBehavior> interactionBehaviors;
    protected bool wasInteracted;

    [ContextMenu("Interact")]
    public void Interact()
    {
        StartCoroutine(Interaction(new InteractionPassData(wasInteracted)));
        wasInteracted = true;
    }

    private IEnumerator Interaction(InteractionPassData data)
    {
        foreach (var behavior in interactionBehaviors)
        {
            yield return new WaitForSeconds(behavior.DelayBefore);
            behavior.Interact(data);
            yield return new WaitUntil(() => behavior.Complete);
            yield return new WaitForSeconds(behavior.DelayAfter);
        }
    }
}

public abstract class ItemInteractionBehavior : MonoBehaviour
{
    public float DelayBefore;
    public float DelayAfter;
    public bool Complete;

    public abstract void Interact(InteractionPassData data);
}

public class InteractionPassData
{
    public bool WasInteractedBefore;

    public InteractionPassData(bool wasInteractedBefore)
    {
        this.WasInteractedBefore = wasInteractedBefore;
    }

    public InteractionPassData()
    {
        WasInteractedBefore = false;
    }
}