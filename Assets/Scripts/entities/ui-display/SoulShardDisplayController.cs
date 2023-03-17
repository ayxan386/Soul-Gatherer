using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoulShardDisplayController : MonoBehaviour
{
    [SerializeField] private Transform cellHolder;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI desc;
    private SoulShardDisplayer[] cells;
    private BaseAbility currentSelectedAbility;

    private void Start()
    {
        EventStore.Instance.OnPlayerAbilityDisplayerClick += OnPlayerAbilityDisplayerClick;
        EventStore.Instance.OnPlayerAbilityModify += OnPlayerAbilityModify;
        ToggleElements(false);
        cells = cellHolder.GetComponentsInChildren<SoulShardDisplayer>();
    }

    private void OnPlayerAbilityModify(BaseAbility ability)
    {
        if (currentSelectedAbility != null && currentSelectedAbility.Id == ability.Id)
            UpdateCells(ability);
    }

    private void OnDestroy()
    {
        EventStore.Instance.OnPlayerAbilityDisplayerClick -= OnPlayerAbilityDisplayerClick;
        EventStore.Instance.OnPlayerAbilityModify -= OnPlayerAbilityModify;
    }

    private void ToggleElements(bool state)
    {
        icon.enabled = state;
        desc.enabled = state;
    }

    private void OnPlayerAbilityDisplayerClick(AbilityDisplayer displayer)
    {
        if (displayer.type != AbilityDisplayType.ModificationMenu)
            return;
        var ability = PlayerAbilityReferenceKeeper.PlayerAbilities[displayer.id];
        ToggleElements(true);
        currentSelectedAbility = ability;
        icon.sprite = ability.Icon;
        desc.text = ability.Desc;
        UpdateCells(ability);
    }

    private void UpdateCells(BaseAbility ability)
    {
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