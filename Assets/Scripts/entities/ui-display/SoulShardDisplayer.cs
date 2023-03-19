using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoulShardDisplayer : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,
    ISubmitHandler, ISelectHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI desc;
    [SerializeField] private GameObject descRef;
    [SerializeField] private Color availableColor;
    [SerializeField] private Color lockedColor;
    [SerializeField] private Color filledColor = Color.white;
    [SerializeField] private bool inventoryCell;
    [SerializeField] private Selectable selfSelection;

    private int state;
    private SoulShard soulShard;

    public void DisplaySoulShard(SoulShard soulShard)
    {
        if (soulShard == null) return;
        state = 3;
        gameObject.SetActive(true);
        this.soulShard = soulShard;
        icon.color = filledColor;
        icon.sprite = soulShard.icon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (soulShard.attachedAbility != null && !soulShard.attachedAbility.CanBeModified) return;
        if (inventoryCell && state == 3)
        {
            EventStore.Instance.PublishShardAdd(soulShard);
        }
        else if (!inventoryCell && state == 3)
        {
            EventStore.Instance.PublishShardRemove(soulShard);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (state > 2)
        {
            descRef.SetActive(true);
            desc.text = soulShard.description;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descRef.SetActive(false);
    }

    public void DisplayAsAvailable()
    {
        gameObject.SetActive(!inventoryCell);
        state = 2;
        icon.color = availableColor;
        icon.sprite = null;
    }

    public void DisplayAsLocked()
    {
        gameObject.SetActive(!inventoryCell);
        state = 1;
        icon.color = lockedColor;
        icon.sprite = null;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (soulShard.attachedAbility != null && !soulShard.attachedAbility.CanBeModified) return;
        if (inventoryCell && state == 3)
        {
            EventStore.Instance.PublishShardAdd(soulShard);
        }
        else if (!inventoryCell && state == 3)
        {
            EventStore.Instance.PublishShardRemove(soulShard);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (state > 2)
        {
            descRef.SetActive(true);
            desc.text = soulShard.description;
        }
    }
}