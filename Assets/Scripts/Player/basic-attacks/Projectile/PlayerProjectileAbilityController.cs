using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerProjectileAbilityController : MonoBehaviour
{
    [FormerlySerializedAs("basicAbilityDetails")] [SerializeField] private ProjectileAbility projectileAbilityDetails;
    [SerializeField] private Transform launchPoint;
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioClip launchSound;

    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitUntil(() => GlobalStateManager.Instance.CurrentState == GameState.Running);
            yield return new WaitUntil(() => Input.GetButton("Fire1"));

            projectileAbilityDetails.LaunchAttack(launchPoint);
            if (launchSound && soundSource)
            {
                soundSource.PlayOneShot(launchSound);
            }

            yield return new WaitForSeconds(projectileAbilityDetails.GetCooldown());
        }
    }
}