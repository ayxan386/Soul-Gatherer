using UnityEngine;

public class MobSounds : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip sound;
    [SerializeField] [Range(0, 1f)] private float chance = 1f;
    [SerializeField] private float cooldown;

    private bool canPlay = true;

    public void MakeSound()
    {
        if (Random.value <= chance && canPlay)
        {
            canPlay = false;
            source.PlayOneShot(sound);
            Invoke("ResetCooldown", cooldown);
        }
    }

    private void ResetCooldown()
    {
        canPlay = true;
    }
}