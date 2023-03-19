using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalStateManager : MonoBehaviour
{
    private Stack<string> pauseLock;
    public GameState CurrentState { get; private set; }
    public ControlSchemes CurrentScheme { get; private set; } = ControlSchemes.Standard;

    public static GlobalStateManager Instance;

    private void Awake()
    {
        Instance = this;
        pauseLock = new Stack<string>();
    }

    public void RunningGame(string lockerName = "default")
    {
        if (pauseLock.Count > 0 && pauseLock.Peek() == lockerName)
        {
            pauseLock.Pop();
        }

        if (pauseLock.Count == 0)
        {
            CurrentState = GameState.Running;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            FindNextSelectable();
        }
    }

    public void PausedGame(string lockerName = "default")
    {
        pauseLock.Push(lockerName);
        CurrentState = GameState.Paused;
        if (CurrentScheme != ControlSchemes.Gamepad)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ChangeScheme(string newScheme)
    {
        CurrentScheme = newScheme == "Gamepad" ? ControlSchemes.Gamepad : ControlSchemes.Standard;
        if (CurrentScheme == ControlSchemes.Standard && CurrentState == GameState.Paused)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void FindNextSelectable()
    {
        foreach (var selectable in Selectable.allSelectablesArray)
        {
            if (selectable.IsInteractable())
            {
                selectable.Select();
                break;
            }
        }
    }
}

public enum GameState
{
    Running,
    Paused
}

public enum ControlSchemes
{
    Standard,
    Gamepad
}