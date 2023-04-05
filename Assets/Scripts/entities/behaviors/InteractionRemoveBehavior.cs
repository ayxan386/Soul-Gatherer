public class InteractionRemoveBehavior : ItemInteractionBehavior
{
    public override void Interact(InteractionPassData data)
    {
        data.reference.Interactable = false;
        Complete = true;
    }
}