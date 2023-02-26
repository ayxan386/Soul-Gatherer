using System.Collections;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{
    [SerializeField] private BaseAbility ability;
    [SerializeField] private Transform launchPoint;
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioClip launchSound;
    [SerializeField] private string buttonName;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => ability != null && !string.IsNullOrEmpty(ability.Id));
        EventStore.Instance.PublishPlayerAbilityAdd(ability);

        while (true)
        {
            yield return new WaitUntil(() => GlobalStateManager.Instance.CurrentState == GameState.Running);
            yield return new WaitUntil(() => Input.GetButton(buttonName));

            if (GlobalStateManager.Instance.CurrentState == GameState.Running)
            {
                ability.CastAbility(launchPoint);
                if (launchSound && soundSource)
                {
                    soundSource.PlayOneShot(launchSound);
                }
            }

            yield return new WaitForSeconds(ability.GetCooldown());
        }
    }
}