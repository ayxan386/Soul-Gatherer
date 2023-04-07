using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObtainedItemDisplayer : MonoBehaviour
{
    [SerializeField] private new TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI count;
    [SerializeField] private Image icon;

    public void Display(ObtainedEntity entity)
    {
        if (entity == null) return;
        name.text = entity.data.name;
        count.text = $"x {entity.count}";
        icon.sprite = entity.data.smallDisplayIcon;
    }
}