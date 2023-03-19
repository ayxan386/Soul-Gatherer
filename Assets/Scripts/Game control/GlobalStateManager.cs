using System.Collections.Generic;
using UnityEngine;

public class GlobalStateManager : MonoBehaviour
{
    private Stack<string> pauseLock;
    public GameState CurrentState { get; private set; }

    public static GlobalStateManager Instance;

    private void Awake()
    {
        Instance = this;
        pauseLock = new Stack<string>();
    }

    public void RunningGame(string lockerName = "default")
    {
        if (pauseLock.Peek() == lockerName)
        {
            pauseLock.Pop();
        }

        if (pauseLock.Count == 0)
        {
            CurrentState = GameState.Running;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void PausedGame(string lockerName = "default")
    {
        pauseLock.Push(lockerName);
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