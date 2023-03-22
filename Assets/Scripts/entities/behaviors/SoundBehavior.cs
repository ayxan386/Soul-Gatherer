using UnityEngine;

public class SoundBehavior : ItemInteractionBehavior
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private bool useClip;
    [SerializeField] private bool useItemInteractionSource;

    public override void Interact(InteractionPassData data)
    {
        if (data.WasInteractedBefore) return;
        if (useClip)
        {
            if (useItemInteractionSource)
            {
                ItemInteraction.Instance.ItemInteractionSound.PlayOneShot(audioClip);
            }
            else
            {
                audioSource.PlayOneShot(audioClip);
            }
        }
        else
        {
            audioSource.Play();
        }

        Complete = true;
    }
}