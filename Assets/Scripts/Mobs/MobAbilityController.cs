using System.Collections;
using UnityEngine;

public class MobAbilityController : MonoBehaviour
{
    [SerializeField] private Transform launchPoint;
    [SerializeField] private ProjectileAbility ability;
    [SerializeField] private AOEAbility aoeAbility;
    [SerializeField] private float physicalDamage;

    private bool canAttack = true;

    private void Start()
    {
        if (LevelLoader.Instance != null)
        {
            var mult = (LevelLoader.Instance ? LevelLoader.Instance.CalculateDifficulty() : 1);
            if (ability != null) ability.IncreaseDamage(mult);
            if (aoeAbility != null)
            {
                aoeAbility.IncreaseDamage(mult);
            }
        }
    }

    public void ShootProjectile()
    {
        if (!canAttack) return;
        canAttack = false;
        ability.CastAbility(launchPoint);
        StartCoroutine(Cooldown(ability.GetCooldown()));
    }

    public void CastAbility()
    {
        if (!canAttack) return;
        canAttack = false;
        aoeAbility.CastAbility(launchPoint);
        StartCoroutine(Cooldown(aoeAbility.GetCooldown()));
    }

    public void MeleeAttack()
    {
        print("Hitting player for : " + physicalDamage);
    }

    private IEnumerator Cooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }
}

public interface IModifiableEntityAbility
{
    public void IncreaseDamage(float mult);
}