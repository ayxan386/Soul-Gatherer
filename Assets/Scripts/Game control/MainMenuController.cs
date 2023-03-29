using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI startText;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject abandonButtonHolder;
    [SerializeField] private Button abandonButton;
    [SerializeField] private Button helpButton;
    [SerializeField] private GameObject helpMenu;

    void Start()
    {
        startButton.onClick.AddListener(() => StartGame());
        settingsButton.onClick.AddListener(() => SettingsMenu());
        quitButton.onClick.AddListener(() => QuitGame());
        abandonButton.onClick.AddListener(() => AbandonCampaign());
        helpButton.onClick.AddListener(() => ChangeHelpMenuState(true));

        if (LevelLoader.Instance == null)
        {
            levelLoader = Instantiate(levelLoader);
        }
        else
        {
            levelLoader = LevelLoader.Instance;
        }

        CheckForActiveCampaign();
    }

    public void ChangeHelpMenuState(bool state)
    {
        helpMenu.SetActive(state);
        SelectionController.Instance.FindNextSelectable();
    }

    private void StartGame()
    {
        if (!levelLoader.HasActiveCampaign())
        {
            levelLoader.GenerateCampaign();
        }


        var currentLevel = levelLoader.GetCurrentLevel();
        SceneManager.LoadScene(currentLevel.levelName);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void SettingsMenu()
    {
        settingsMenu.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
    }

    private void AbandonCampaign()
    {
        levelLoader.AbandonRun();
        CheckForActiveCampaign();
    }

    private void CheckForActiveCampaign()
    {
        if (levelLoader.HasActiveCampaign())
        {
            startText.text = "Continue";
            abandonButtonHolder.SetActive(true);
        }
        else
        {
            abandonButtonHolder.SetActive(false);
            startText.text = "Start new game";
        }
    }
}