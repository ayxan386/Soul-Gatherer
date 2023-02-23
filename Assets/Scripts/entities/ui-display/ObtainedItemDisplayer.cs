using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObtainedItemDisplayer : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private new TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI count;
    [SerializeField] private Image icon;

    private ObtainedEntity entityData;

    public void Display(ObtainedEntity entity)
    {
        if (entity == null) return;

        entityData = entity;
        name.text = entity.data.name;
        count.text = $"x {entity.count}";
        icon.sprite = entity.data.smallDisplayIcon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventStore.Instance.PublishEntityObtainedClick(entityData);
    }
}