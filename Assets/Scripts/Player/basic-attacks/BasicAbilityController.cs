using System.Collections;
using UnityEngine;

public class BasicAbilityController : MonoBehaviour
{
    [SerializeField] private BasicAbility basicAbilityDetails;
    [SerializeField] private Transform launchPoint;
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioClip launchSound;

    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitUntil(() => GlobalStateManager.Instance.CurrentState == GameState.Running);
            yield return new WaitUntil(() => Input.GetButton("Fire1"));

            basicAbilityDetails.LaunchAttack(launchPoint);
            if (launchSound && soundSource)
            {
                soundSource.PlayOneShot(launchSound);
            }

            yield return new WaitForSeconds(basicAbilityDetails.GetCooldown());
        }
    }
}