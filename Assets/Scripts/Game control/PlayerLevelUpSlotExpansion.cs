using UnityEngine;

public class PlayerLevelUpSlotExpansion : MonoBehaviour
{
    private const string PauseLockName = "Level-up-reward";
    [SerializeField] private GameObject wrapper;
    [SerializeField] private Transform uiHolder;
    [SerializeField] private AbilityDisplayer abilityDisplayer;

    private int leftRewards;

    void Start()
    {
        EventStore.Instance.OnPlayerLevelUp += OnPlayerLevelUp;
        EventStore.Instance.OnPlayerAbilityDisplayerClick += AbilityDisplayerClick;
    }


    void OnDisable()
    {
        EventStore.Instance.OnPlayerLevelUp -= OnPlayerLevelUp;
        EventStore.Instance.OnPlayerAbilityDisplayerClick -= AbilityDisplayerClick;
    }

    private void OnPlayerLevelUp(int obj)
    {
        leftRewards++;
        if (leftRewards == 1)
        {
            Time.timeScale = 0;
            GlobalStateManager.Instance.PausedGame(PauseLockName);
            ShowRewards();
        }
    }

    private void ShowRewards()
    {
        wrapper.SetActive(true);
        var playerAbilities = PlayerAbilityReferenceKeeper.PlayerAbilities.Values;
        int childIndex = 0;
        foreach (var playerAbility in playerAbilities)
        {
            if (!playerAbility.CanBeModified) continue;

            if (childIndex < uiHolder.childCount)
            {
                var currentAbility = uiHolder.GetChild(childIndex++).GetComponent<AbilityDisplayer>();
                currentAbility.gameObject.SetActive(true);
                currentAbility.DisplayAbility(playerAbility);
                currentAbility.type = AbilityDisplayType.RewardMenu;
            }
            else
            {
                childIndex++;
                var displayer = Instantiate(abilityDisplayer, uiHolder);
                displayer.DisplayAbility(playerAbility);
                displayer.type = AbilityDisplayType.RewardMenu;
            }
        }
    }

    private void AbilityDisplayerClick(AbilityDisplayer displayer)
    {
        if (displayer.type != AbilityDisplayType.RewardMenu) return;
        var ability = PlayerAbilityReferenceKeeper.PlayerAbilities[displayer.id];

        ability.ExpandSlotCount();
        leftRewards--;
        if (leftRewards > 0) ShowRewards();
        else
        {
            DisableUi();
            Time.timeScale = 1;
            GlobalStateManager.Instance.RunningGame(PauseLockName);
        }
    }

    private void DisableUi()
    {
        for (int i = 0; i < uiHolder.childCount; i++)
        {
            uiHolder.GetChild(i).gameObject.SetActive(false);
        }

        wrapper.SetActive(false);
    }
}