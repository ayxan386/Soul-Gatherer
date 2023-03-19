using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuRef;
    [SerializeField] private GameObject playerHealthUI;
    private bool isPaused;

    private void OnEnable()
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
    }
}