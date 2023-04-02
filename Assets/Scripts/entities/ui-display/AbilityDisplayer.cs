using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityDisplayer : MonoBehaviour, IPointerClickHandler, ISubmitHandler, IPointerEnterHandler,
    ISelectHandler
{
    public string id;
    public AbilityDisplayType type;
    public TextMeshProUGUI descText;
    public string price;
    public Color priceTextColor;
    [SerializeField] private Image icon;
    [SerializeField] private Selectable selfSelection;
    [SerializeField] private bool hasPrice;
    [SerializeField] private GameObject priceWrapper;
    [SerializeField] private TextMeshProUGUI priceText;

    private void Start()
    {
        if (string.IsNullOrEmpty(id)) return;
        DisplayAbility(PlayerAbilityReferenceKeeper.PlayerAbilities[id]);
        selfSelection.Select();
    }

    public void DisplayAbility(BaseAbility playerAbility)
    {
        if (playerAbility == null) return;
        id = playerAbility.Id;
        icon.sprite = playerAbility.Icon;

        priceWrapper.SetActive(hasPrice);
        if (hasPrice)
        {
            priceText.text = price;
            priceText.color = priceTextColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventStore.Instance.PublishPlayerAbilityDisplayerClick(this);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        EventStore.Instance.PublishPlayerAbilityDisplayerClick(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DescribeAbility();
    }

    public void OnSelect(BaseEventData eventData)
    {
        DescribeAbility();
    }

    private void DescribeAbility()
    {
        if (descText == null) return;
        if (descText.transform.parent)
        {
            descText.transform.parent.gameObject.SetActive(true);
        }

        descText.alpha = 1;
        descText.text = PlayerAbilityReferenceKeeper.PlayerAbilities[id].GetDescription();
    }
}

public enum AbilityDisplayType
{
    ModificationMenu,
    RewardMenu,
    ShopMenu
}