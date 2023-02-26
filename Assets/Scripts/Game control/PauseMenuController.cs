using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuRef;
    private bool isPaused;

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
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
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        pauseMenuRef.SetActive(false);
        GlobalStateManager.Instance.RunningGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenuRef.SetActive(true);
        GlobalStateManager.Instance.PausedGame();
    }
}