using UnityEngine;

public class SelfDestructBehavior : ItemInteractionBehavior
{
    [SerializeField] private GameObject target;

    public override void Interact(InteractionPassData data)
    {
        Complete = true;
        Destroy(target);
    }
}