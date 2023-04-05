using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private Toggle shadowToggle;

    private const string ProcessingShadows = "processing-shadows";
    private Volume globalVolume;

    private void Start()
    {
        shadowToggle.onValueChanged.AddListener(OnProcessingStateChange);
        GameObject.FindWithTag("GlobalVolume")?.TryGetComponent(out globalVolume);
        UpdateShadowProcessing(PlayerPrefs.GetInt(ProcessingShadows) == 1);
    }

    public void OnProcessingStateChange(bool state)
    {
        PlayerPrefs.SetInt(ProcessingShadows, state ? 1 : 0);
        UpdateShadowProcessing(state);
    }

    private void UpdateShadowProcessing(bool state)
    {
        shadowToggle.isOn = state;
        if (globalVolume == null) return;
        foreach (var profileComponent in globalVolume.profile.components)
        {
            if (profileComponent is ShadowsMidtonesHighlights)
            {
                profileComponent.active = state;
                break;
            }
        }
    }
}