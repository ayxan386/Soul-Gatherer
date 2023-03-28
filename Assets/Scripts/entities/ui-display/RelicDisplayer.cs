using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RelicDisplayer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,
    ISubmitHandler
{
    [SerializeField] private Image icon;
    private TextMeshProUGUI description;

    private BaseRelic attachedRelic;

    public void UpdateUi(BaseRelic relic, TextMeshProUGUI passedDescription)
    {
        if (relic == null) return;
        description = passedDescription;
        attachedRelic = relic;
        icon.sprite = relic.Icon;
        description.text = relic.GetDescription();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        description.alpha = 1;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        description.alpha = 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ApplyRelicEffect();
    }


    public void OnSubmit(BaseEventData eventData)
    {
        ApplyRelicEffect();
    }

    private void ApplyRelicEffect()
    {
        description.alpha = 0;
        attachedRelic.RelicUsed();
    }
}