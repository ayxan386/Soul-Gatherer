using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI subtitle;
    [SerializeField] private string[] titleTexts;
    [SerializeField] private string[] subtitleTexts;
    [SerializeField] private Color[] backgroundColors;

    private const string EndSceneName = "EndScene";
    private const string MainMenuName = "MainMenu";

    private static bool result;

    IEnumerator Start()
    {
        LevelLoader.Instance.AbandonRun();
        var index = result ? 0 : 1;
        title.text = titleTexts[index];
        subtitle.text = subtitleTexts[index];
        Camera.main.backgroundColor = backgroundColors[index];
        yield return new WaitUntil(() => Input.anyKey);
        SceneManager.LoadScene(MainMenuName);
    }


    public static void LoadEndScene(bool isVictory = true)
    {
        GlobalStateManager.Instance?.RunningGame();
        result = isVictory;
        SceneManager.LoadScene(EndSceneName);
    }
}