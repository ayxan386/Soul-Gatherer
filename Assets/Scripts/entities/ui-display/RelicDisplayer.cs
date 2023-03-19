using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RelicDisplayer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private GameObject descriptionHolder;

    // private BaseRelic attachedRelic;

    public void UpdateUi(BaseRelic relic)
    {
        if (relic == null) return;
        // attachedRelic = relic;
        icon.sprite = relic.Icon;
        description.text = relic.GetDescription();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionHolder.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionHolder.SetActive(false);
    }
}