using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityDisplayer : MonoBehaviour, IPointerClickHandler, ISubmitHandler
{
    public string id;
    public AbilityDisplayType type;
    [SerializeField] private Image icon;
    [SerializeField] private Selectable selfSelection;

    private void Start()
    {
        DisplayAbility(PlayerAbilityReferenceKeeper.PlayerAbilities[id]);
        selfSelection.Select();
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

    public void OnSubmit(BaseEventData eventData)
    {
        EventStore.Instance.PublishPlayerAbilityDisplayerClick(this);
    }
}

public enum AbilityDisplayType
{
    ModificationMenu,
    RewardMenu
}