using UnityEngine;

public class ObtainRelicBehavior : ItemInteractionBehavior
{
    [SerializeField] private BaseRelic relic;


    public override void Interact(InteractionPassData data)
    {
        if (data.WasInteractedBefore)
        {
            Complete = true;
            return;
        }

        var newRelic = Instantiate(relic);
        EventStore.Instance.PublishRelicObtained(newRelic);
        Complete = true;
    }
}