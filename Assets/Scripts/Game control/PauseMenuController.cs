using System.Collections;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuRef;
    [SerializeField] private GameObject playerHealthUI;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject levelLoader;
    private bool isPaused;

    private IEnumerator Start()
    {
        EventStore.Instance.OnPauseMenu += OnPauseMenu;
        if (LevelLoader.Instance == null)
        {
            Instantiate(levelLoader);
        }

        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(1);
        PlayerDataManager.LoadData();
        yield return new WaitForSeconds(1);
        loadingScreen.SetActive(false);
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
        PlayerDataManager.SaveData();
        IntermediateLevelLoader.LoadLevel("MainMenu");
    }
}