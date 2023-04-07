using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenuUi;
    private const string ProcessingShadows = "processing-shadows";
    private const string GamepadsensitivityMult = "GamepadSensitivityMult";

    private Toggle shadowToggle;
    private Volume globalVolume;
    private Slider gamepadSensitivitySlider;
    private TextMeshProUGUI gamepadSenValueText;

    private void Start()
    {
        GameObject.FindWithTag("GlobalVolume")?.TryGetComponent(out globalVolume);
        gamepadSensitivitySlider = settingsMenuUi.transform.Find("GamepadSensitivity").GetComponent<Slider>();
        gamepadSenValueText = gamepadSensitivitySlider.transform.Find("value").GetComponent<TextMeshProUGUI>();
        shadowToggle = settingsMenuUi.transform.Find("PostProcessingEffects").GetComponent<Toggle>();

        shadowToggle.onValueChanged.AddListener(OnProcessingStateChange);
        gamepadSensitivitySlider.onValueChanged.AddListener(OnGamepadSensitivityChanged);

        settingsMenuUi.SetActive(false);

        UpdateShadowProcessing(PlayerPrefs.GetInt(ProcessingShadows) == 1);
        OnGamepadSensitivityChanged(PlayerPrefs.GetFloat(GamepadsensitivityMult));
    }

    private void OnGamepadSensitivityChanged(float val)
    {
        PlayerPrefs.SetFloat(GamepadsensitivityMult, val);
        gamepadSensitivitySlider.value = val;
        gamepadSenValueText.text = $"x {val:N1}";
        if (Camera.main.TryGetComponent(out CameraMovement cm))
        {
            cm.GamepadSensitivityMult = val;
        }
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