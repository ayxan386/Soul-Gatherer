using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugHelper : MonoBehaviour
{
    [SerializeField] private EntityData relic;
    [SerializeField] private int goldAmount;
    [SerializeField] [Range(0, 1f)] private float brit;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void GiveRelic()
    {
        EventStore.Instance.PublishEntityObtainedClick(new ObtainedEntity(relic, 1));
    }

    public void GiveGold()
    {
        EventStore.Instance.PublishGoldSpent(-goldAmount);
    }

    public void ChangeBrightness()
    {
        Screen.brightness = brit;
    }
}