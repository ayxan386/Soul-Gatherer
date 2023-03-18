using UnityEngine;

public class ModificationMenuAbilityDisplayerController : MonoBehaviour
{
    [SerializeField] private AbilityDisplayer displayerPrefab;
    [SerializeField] private Transform uiHolder;

    void Start()
    {
        EventStore.Instance.OnPlayerAbilityAdd += onPlayerAbilityAdd;
    }

    private void onPlayerAbilityAdd(BaseAbility ability)
    {
        var abilityDisplayer = Instantiate(displayerPrefab, uiHolder);
        abilityDisplayer.id = ability.Id;
        abilityDisplayer.type = AbilityDisplayType.ModificationMenu;
    }

    private void OnDestroy()
    {
        EventStore.Instance.OnPlayerAbilityAdd -= onPlayerAbilityAdd;
    }
}