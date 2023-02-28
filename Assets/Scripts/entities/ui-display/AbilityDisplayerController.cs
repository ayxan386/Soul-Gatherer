using UnityEngine;

public class AbilityDisplayerController : MonoBehaviour
{
    [SerializeField] private AbilityDisplayer displayerPrefab;
    [SerializeField] private Transform uiHolder;

    void Start()
    {
        EventStore.Instance.OnPlayerAbilityAdd += onPlayerAbilityAdd;
    }

    private void onPlayerAbilityAdd(BaseAbility ability)
    {
        Instantiate(displayerPrefab, uiHolder).id = ability.Id;
    }

    private void OnDestroy()
    {
        EventStore.Instance.OnPlayerAbilityAdd -= onPlayerAbilityAdd;
    }
}