using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntermediateLevelLoader : MonoBehaviour
{
    private const string LevelName = "LevelToLoad";
    private const string IntermediateLevelName = "IntermediateScene";

    [SerializeField] private GameObject message;

    private IEnumerator Start()
    {
        Time.timeScale = 1;
        print("Started loading");
        var sceneName = PlayerPrefs.GetString(LevelName);
        print("Scene to load: " + sceneName);
        var sceneAsync = SceneManager.LoadSceneAsync(sceneName);
        sceneAsync.allowSceneActivation = false;
        while (!sceneAsync.isDone && sceneAsync.progress < 0.9f)
        {
            print(sceneAsync.progress);
            yield return new WaitForSeconds(0.1f);
        }

        print("Load complete");
        message.SetActive(true);
        yield return new WaitUntil(() => Input.anyKey);
        sceneAsync.allowSceneActivation = true;
    }

    public static void LoadLevel(string levelName)
    {
        PlayerPrefs.SetString(LevelName, levelName);
        SceneManager.LoadScene(IntermediateLevelName);
    }
}