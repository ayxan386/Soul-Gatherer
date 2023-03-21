using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuRef;
    [SerializeField] private GameObject playerHealthUI;
    [SerializeField] private GameObject settingsMenu;
    private bool isPaused;

    private void Start()
    {
        EventStore.Instance.OnPauseMenu += OnPauseMenu;
    }

    private void OnDisable()
    {
        EventStore.Instance.OnPauseMenu -= OnPauseMenu;
    }

    void OnPauseMenu()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ContinueGame();
        }
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        playerHealthUI.SetActive(true);
        pauseMenuRef.SetActive(false);
        GlobalStateManager.Instance.RunningGame("PauseMenu");
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        playerHealthUI.SetActive(false);
        pauseMenuRef.SetActive(true);
        GlobalStateManager.Instance.PausedGame("PauseMenu");
        SelectionController.Instance.FindNextSelectable();
    }

    public void SettingsMenu()
    {
        settingsMenu.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
        SelectionController.Instance.FindNextSelectable();
    }

    public void MainMenu()
    {
        IntermediateLevelLoader.LoadLevel("MainMenu");
    }
}