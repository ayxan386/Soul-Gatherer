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
        ShowRewards();
    }

    private void ShowRewards()
    {
        wrapper.SetActive(true);
        GlobalStateManager.Instance.PausedGame(PauseLockName);
        Time.timeScale = 0;
        var playerAbilities = PlayerAbilityReferenceKeeper.PlayerAbilities.Values;
        foreach (var playerAbility in playerAbilities)
        {
            if (playerAbility.CanBeModified)
            {
                var displayer = Instantiate(abilityDisplayer, uiHolder);
                displayer.id = playerAbility.Id;
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
            Destroy(uiHolder.GetChild(i).gameObject);
        }

        wrapper.SetActive(false);
    }
}