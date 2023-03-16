using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    [SerializeField] protected List<ItemInteractionBehavior> interactionBehaviors;
    public bool Interactable { get; set; } = true;
    protected bool wasInteracted;
    private bool inProgress;

    [ContextMenu("Interact")]
    public void Interact()
    {
        if (!Interactable) return;
        if (inProgress) return;
        print("Interacting");
        inProgress = true;
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

        inProgress = false;
    }
}

public abstract class ItemInteractionBehavior : MonoBehaviour
{
    public float DelayBefore;
    public float DelayAfter;
    public bool Complete;

    [ContextMenu("Get items")]
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