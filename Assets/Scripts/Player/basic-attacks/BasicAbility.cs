using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BasicAbility", order = 1)]
public class BasicAbility : ScriptableObject
{
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private float cooldown;

    public void LaunchAttack(Transform attackPoint)
    {
        Instantiate(attackPrefab, attackPoint.position, Quaternion.LookRotation(attackPoint.forward));
    }

    public float GetCooldown()
    {
        return cooldown;
    }
}
