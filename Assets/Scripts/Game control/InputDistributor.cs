using UnityEngine;
using UnityEngine.InputSystem;

public class InputDistributor : MonoBehaviour
{
    private void OnPauseMenu()
    {
        EventStore.Instance.PublishPauseMenu();
    }

    private void OnControlsChanged(PlayerInput playerInput)
    {
        GlobalStateManager.Instance?.ChangeScheme(playerInput.currentControlScheme);
    }
}