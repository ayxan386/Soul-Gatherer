using UnityEngine;

public class GlobalStateManager : MonoBehaviour
{
    public GameState CurrentState { get; private set; }

    public static GlobalStateManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void RunningGame()
    {
        CurrentState = GameState.Running;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PausedGame()
    {
        CurrentState = GameState.Paused;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}

public enum GameState
{
    Running,
    Paused
}