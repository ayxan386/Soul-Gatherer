using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenuUi;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string musicGroupName;
    [SerializeField] private string sfxGroupName = "foregroundVolume";

    private const string ProcessingShadows = "processing-shadows";
    private const string GamepadsensitivityMult = "GamepadSensitivityMult";
    private const string MusicVolume = "MusicVolume";
    private const string SFXVolume = "SFXVolume";

    private Toggle shadowToggle;
    private Volume globalVolume;
    private Slider gamepadSensitivitySlider;
    private Slider musicVolumeSlider;
    private Slider sfxVolumeSlider;
    private TextMeshProUGUI gamepadSenValueText;
    private TextMeshProUGUI musicVolumeValueText;
    private TextMeshProUGUI sfxVolumeValueText;

    private void Start()
    {
        GameObject.FindWithTag("GlobalVolume")?.TryGetComponent(out globalVolume);
        gamepadSensitivitySlider = settingsMenuUi.transform.Find("GamepadSensitivity").GetComponent<Slider>();
        musicVolumeSlider = settingsMenuUi.transform.Find("MusicVolume").GetComponent<Slider>();
        sfxVolumeSlider = settingsMenuUi.transform.Find("SFXVolume").GetComponent<Slider>();
        gamepadSenValueText = gamepadSensitivitySlider.transform.Find("value").GetComponent<TextMeshProUGUI>();
        musicVolumeValueText = musicVolumeSlider.transform.Find("value").GetComponent<TextMeshProUGUI>();
        sfxVolumeValueText = sfxVolumeSlider.transform.Find("value").GetComponent<TextMeshProUGUI>();
        shadowToggle = settingsMenuUi.transform.Find("PostProcessingEffects").GetComponent<Toggle>();

        shadowToggle.onValueChanged.AddListener(OnProcessingStateChange);
        gamepadSensitivitySlider.onValueChanged.AddListener(OnGamepadSensitivityChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChange);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChange);

        settingsMenuUi.SetActive(false);

        UpdateShadowProcessing(PlayerPrefs.GetInt(ProcessingShadows) == 1);
        OnGamepadSensitivityChanged(PlayerPrefs.HasKey(GamepadsensitivityMult)
            ? PlayerPrefs.GetFloat(GamepadsensitivityMult) : 1);
        OnMusicVolumeChange(PlayerPrefs.HasKey(MusicVolume) ? PlayerPrefs.GetFloat(MusicVolume) : 0.5f);
        OnSFXVolumeChange(PlayerPrefs.HasKey(SFXVolume) ? PlayerPrefs.GetFloat(SFXVolume) : 0.5f);
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

    private void OnMusicVolumeChange(float val)
    {
        PlayerPrefs.SetFloat(MusicVolume, val);
        musicVolumeSlider.value = val;
        musicVolumeValueText.text = $"{val * 100:N1} %";
        audioMixer.SetFloat(musicGroupName, Mathf.Log(val) * 20);
    }

    private void OnSFXVolumeChange(float val)
    {
        PlayerPrefs.SetFloat(SFXVolume, val);
        sfxVolumeSlider.value = val;
        sfxVolumeValueText.text = $"{val * 100:N1} %";
        audioMixer.SetFloat(sfxGroupName, Mathf.Log(val) * 20);
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