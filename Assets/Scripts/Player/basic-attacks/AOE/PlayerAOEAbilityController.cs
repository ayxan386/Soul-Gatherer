using System.Collections;
using UnityEngine;

public class PlayerAOEAbilityController : MonoBehaviour
{
    [SerializeField] private AOEAbility aoeAbility;
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioClip launchSound;

    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitUntil(() => GlobalStateManager.Instance.CurrentState == GameState.Running);
            yield return new WaitUntil(() => Input.GetButton("Fire2"));

            aoeAbility.CastAbility(transform);
            
            if (launchSound && soundSource)
            {
                soundSource.PlayOneShot(launchSound);
            }

            yield return new WaitForSeconds(aoeAbility.GetCooldown());
        }
    }
}