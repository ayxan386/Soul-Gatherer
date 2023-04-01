using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugHelper : MonoBehaviour
{
    [SerializeField] private EntityData relic;
    private int count;
    private float total;

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
}