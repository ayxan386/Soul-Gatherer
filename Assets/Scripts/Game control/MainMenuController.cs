using System;
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

    void Start()
    {
        startButton.onClick.AddListener(() => StartGame());
        settingsButton.onClick.AddListener(() => SettingsMenu());
        quitButton.onClick.AddListener(() => QuitGame());

        if (levelLoader.Instance == null)
        {
            levelLoader = Instantiate(levelLoader);
        }
        else
        {
            levelLoader = levelLoader.Instance;
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
        throw new NotImplementedException();
    }


    // Update is called once per frame
    void Update()
    {
    }
}