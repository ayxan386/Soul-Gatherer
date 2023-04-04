using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugHelper : MonoBehaviour
{
    [SerializeField] private EntityData relic;
    [SerializeField] private int goldAmount;
    private int count;
    private float total;
    [SerializeField] [Range(0, 1f)] private float brit;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        total += 1f / Time.deltaTime;
        count++;
        // print("FPS: " + total / count);

        if (count >= 10000)
        {
            count = 0;
            total = 0;
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