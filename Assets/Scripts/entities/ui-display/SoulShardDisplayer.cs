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

    private void Start()
    {
        selfSelection.interactable = false;
    }

    public void DisplaySoulShard(SoulShard soulShard)
    {
        if (soulShard == null) return;
        state = 3;
        selfSelection.interactable = true;
        gameObject.SetActive(true);
        this.soulShard = soulShard;
        icon.color = filledColor;
        icon.sprite = soulShard.icon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CellSubmit();
    }

    private void CellSubmit()
    {
        if (state != 3 && soulShard.attachedAbility != null && !soulShard.attachedAbility.CanBeModified) return;
        if (inventoryCell)
        {
            EventStore.Instance.PublishShardAdd(soulShard);
        }
        else if (!inventoryCell)
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
        selfSelection.interactable = false;
        gameObject.SetActive(!inventoryCell);
        state = 2;
        icon.color = availableColor;
        icon.sprite = null;
    }

    public void DisplayAsLocked()
    {
        selfSelection.interactable = false;
        gameObject.SetActive(!inventoryCell);
        state = 1;
        icon.color = lockedColor;
        icon.sprite = null;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        CellSubmit();
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