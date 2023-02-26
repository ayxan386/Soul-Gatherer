using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityReferenceKeeper : MonoBehaviour
{
    public static Dictionary<string, BaseAbility> PlayerAbilities;

    private void Start()
    {
        PlayerAbilities = new Dictionary<string, BaseAbility>();
        EventStore.Instance.OnPlayerAbilityAdd += OnPlayerAbilityAdd;
    }

    private void OnPlayerAbilityAdd(BaseAbility ability)
    {
        PlayerAbilities.Add(ability.Id, ability);
    }

    private void OnDisable()
    {
        EventStore.Instance.OnPlayerAbilityAdd -= OnPlayerAbilityAdd;
    }
}