using System.Collections;
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

    [Header("Background slides")] [SerializeField]
    private Image backgroundImage;

    [SerializeField] private Sprite[] backgroundSprites;
    [SerializeField] private float rate;
    private int currentSprite = 0;

    void Start()
    {
        startButton.onClick.AddListener(() => StartGame());
        settingsButton.onClick.AddListener(() => SettingsMenu());
        quitButton.onClick.AddListener(() => QuitGame());
        abandonButton.onClick.AddListener(() => AbandonCampaign());
        helpButton.onClick.AddListener(() => ChangeHelpMenuState(true));

        StartCoroutine(SlideShow());

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

    private IEnumerator SlideShow()
    {
        while (true)
        {
            backgroundImage.sprite = backgroundSprites[currentSprite++];
            currentSprite %= backgroundSprites.Length;
            yield return new WaitForSeconds(rate);
        }
    }

    public void ChangeHelpMenuState(bool state)
    {
        helpMenu.SetActive(state);
        helpButton.interactable = !state;
        if (helpButton.interactable) helpButton.Select();
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
        settingsButton.interactable = false;
    }

    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
        settingsButton.interactable = true;
        settingsButton.Select();
    }

    private void AbandonCampaign()
    {
        levelLoader.AbandonRun();
        CheckForActiveCampaign();
        SelectionController.FindNextSelectable();
    }

    private void CheckForActiveCampaign()
    {
        if (levelLoader.HasActiveCampaign())
        {
            startText.text = "Continue";
            startButton.Select();
            abandonButtonHolder.SetActive(true);
        }
        else
        {
            abandonButtonHolder.SetActive(false);
            startText.text = "Start new game";
        }
    }
}