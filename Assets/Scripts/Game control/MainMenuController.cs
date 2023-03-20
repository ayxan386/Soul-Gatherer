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

    void Start()
    {
        startButton.onClick.AddListener(() => StartGame());
        settingsButton.onClick.AddListener(() => SettingsMenu());
        quitButton.onClick.AddListener(() => QuitGame());

        if (LevelLoader.Instance == null)
        {
            levelLoader = Instantiate(levelLoader);
        }
        else
        {
            levelLoader = LevelLoader.Instance;
        }
    }

    private void StartGame()
    {
        if (!levelLoader.HasActiveCampaign())
        {
            levelLoader.GenerateCampaign();
        }
        else
        {
            startText.text = "Continue";
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
}