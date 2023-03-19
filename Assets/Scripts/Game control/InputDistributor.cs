using UnityEngine;

public class InputDistributor : MonoBehaviour
{
    private void OnPauseMenu()
    {
        EventStore.Instance.PublishPauseMenu();
    }
}