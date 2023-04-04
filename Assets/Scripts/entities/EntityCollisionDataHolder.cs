using UnityEngine;

public class EntityCollisionDataHolder : MonoBehaviour, IAbilityAffected
{
    [SerializeField] private bool shouldDestroyProjectiles;

    public void ApplyAbility(AbilityParam details)
    {
    }

    public bool ShouldDestroyAbility(AbilityParam passedParam)
    {
        return passedParam is ProjectileParams && shouldDestroyProjectiles;
    }
}