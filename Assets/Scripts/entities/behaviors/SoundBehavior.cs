using UnityEngine;

public class SoundBehavior : ItemInteractionBehavior
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private bool useClip;

    public override void Interact(InteractionPassData data)
    {
        if (data.WasInteractedBefore) return;
        if (useClip)
        {
            audioSource.PlayOneShot(audioClip);
        }
        else
        {
            audioSource.Play();
        }

        Complete = true;
    }
}