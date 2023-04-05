using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour, ILoadableEntity
{
    [SerializeField] protected List<ItemInteractionBehavior> interactionBehaviors;
    public bool Interactable { get; set; } = true;
    protected bool wasInteracted;
    private bool inProgress;
    [SerializeField] private StringIdHolder assignedId;

    private void Awake()
    {
        assignedId = GetComponent<StringIdHolder>();
    }

    [ContextMenu("Interact")]
    public void Interact()
    {
        print("Interacted");
        if (!Interactable) return;
        if (inProgress) return;
        inProgress = true;
        EventStore.Instance.OnItemInteractionCancelled += OnItemInteractionCancelled;
        StartCoroutine(Interaction(new InteractionPassData(wasInteracted)));
        wasInteracted = true;
    }

    private void OnItemInteractionCancelled(string passedId)
    {
        if (assignedId != null && assignedId.id == passedId)
        {
            inProgress = false;
        }
    }

    private IEnumerator Interaction(InteractionPassData data)
    {
        data.reference = this;
        foreach (var behavior in interactionBehaviors)
        {
            print("Interacting with " + behavior.GetType().Name);
            yield return new WaitForSeconds(behavior.DelayBefore);
            behavior.Interact(data);
            yield return new WaitUntil(() => behavior.Complete);
            yield return new WaitForSeconds(behavior.DelayAfter);
        }

        inProgress = false;
    }

    public void LoadData(LoadableEntityData data)
    {
        wasInteracted = data.wasInteracted;
        Interactable = data.interactable;
    }

    public LoadableEntityData GetData()
    {
        var res = new LoadableEntityData();
        res.wasInteracted = wasInteracted;
        res.interactable = Interactable;
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
    public InteractableItem reference;

    public InteractionPassData(bool wasInteractedBefore)
    {
        this.WasInteractedBefore = wasInteractedBefore;
    }

    public InteractionPassData()
    {
        WasInteractedBefore = false;
    }
}