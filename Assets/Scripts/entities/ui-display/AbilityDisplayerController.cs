using UnityEngine;

public class AbilityDisplayerController : MonoBehaviour
{
    [SerializeField] private AbilityDisplayer displayerPrefab;

    void Start()
    {
        foreach (var abilityKey in PlayerAbilityReferenceKeeper.PlayerAbilities.Keys)
        {
            print("Ability key: " + abilityKey);
            Instantiate(displayerPrefab, transform).id = abilityKey;
        }
    }
}