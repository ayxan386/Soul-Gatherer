using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoulShardDisplayController : MonoBehaviour
{
    [SerializeField] private Transform cellHolder;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI desc;
    private SoulShardDisplayer[] cells;

    private void Start()
    {
        EventStore.Instance.OnPlayerAbilityDisplayerClick += OnPlayerAbilityDisplayerClick;
        ToggleElements(false);
        cells = cellHolder.GetComponentsInChildren<SoulShardDisplayer>();
    }

    private void OnDestroy()
    {
        EventStore.Instance.OnPlayerAbilityDisplayerClick -= OnPlayerAbilityDisplayerClick;
    }

    private void ToggleElements(bool state)
    {
        icon.enabled = state;
        desc.enabled = state;
    }

    private void OnPlayerAbilityDisplayerClick(BaseAbility ability)
    {
        ToggleElements(true);
        icon.sprite = ability.Icon;
        desc.text = ability.Desc;
        for (int i = 0; i < cells.Length; i++)
        {
            if (i < ability.InstalledShards.Count)
            {
                cells[i].DisplaySoulShard(ability.InstalledShards[i]);
            }
            else if (i < ability.AvailableSlots)
            {
                cells[i].DisplayAsAvailable();
            }
            else
            {
                cells[i].DisplayAsLocked();
            }
        }
    }
}