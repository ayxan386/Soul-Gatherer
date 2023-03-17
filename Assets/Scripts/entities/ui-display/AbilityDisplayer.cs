using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityDisplayer : MonoBehaviour, IPointerClickHandler
{
    public string id;
    public AbilityDisplayType type;
    [SerializeField] private Image icon;

    private void Start()
    {
        DisplayAbility(PlayerAbilityReferenceKeeper.PlayerAbilities[id]);
        // EventStore.Instance.OnPlayerAbilityModify += OnPlayerAbilityModify;
    }

    private void OnPlayerAbilityModify(BaseAbility ability)
    {
        if (id == ability.Id) DisplayAbility(ability);
    }

    private void DisplayAbility(BaseAbility playerAbility)
    {
        if (playerAbility == null) return;
        id = playerAbility.Id;
        icon.sprite = playerAbility.Icon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventStore.Instance.PublishPlayerAbilityDisplayerClick(this);
    }

    private void OnDisable()
    {
        // EventStore.Instance.OnPlayerAbilityModify -= OnPlayerAbilityModify;
    }
}

public enum AbilityDisplayType
{
    ModificationMenu,
    RewardMenu
}