using System.Collections;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{
    [SerializeField] private BaseAbility ability;
    [SerializeField] private Transform launchPoint;
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioClip launchSound;
    [SerializeField] private AbilityBinding bindingName;

    private bool canUse = true;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => ability != null && !string.IsNullOrEmpty(ability.Id));
        EventStore.Instance.PublishPlayerAbilityAdd(ability);
    }

    private void UseAbility()
    {
        if (!canUse) return;
        canUse = false;
        if (GlobalStateManager.Instance.CurrentState == GameState.Running)
        {
            ability.CastAbility(launchPoint);
            if (launchSound && soundSource)
            {
                soundSource.PlayOneShot(launchSound);
            }
        }

        StartCoroutine(AbilityCooldown());
    }

    private IEnumerator AbilityCooldown()
    {
        yield return new WaitForSeconds(ability.GetCooldown());
        canUse = true;
    }

    void OnProjectile()
    {
        if (bindingName == AbilityBinding.Projectile)
        {
            UseAbility();
        }
    }

    void OnAOE()
    {
        if (bindingName == AbilityBinding.AOE)
        {
            UseAbility();
        }
    }

    void OnNorth()
    {
        if (bindingName == AbilityBinding.North)
        {
            UseAbility();
        }
    }

    void OnSouth()
    {
        if (bindingName == AbilityBinding.South)
        {
            UseAbility();
        }
    }

    void OnEast()
    {
        if (bindingName == AbilityBinding.East)
        {
            UseAbility();
        }
    }

    void OnWest()
    {
        if (bindingName == AbilityBinding.West)
        {
            UseAbility();
        }
    }
}

public enum AbilityBinding
{
    Projectile,
    AOE,
    North,
    South,
    East,
    West
}