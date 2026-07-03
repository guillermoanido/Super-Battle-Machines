using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    private const string MasterVolumeParam = "MasterVolume";
    private const string MusicVolumeParam = "MusicVolume";
    private const string SfxVolumeParam = "SFXVolume";

    private const string MasterVolumeKey = "settings.volume.master";
    private const string MusicVolumeKey = "settings.volume.music";
    private const string SfxVolumeKey = "settings.volume.sfx";
    private const string ResolutionKey = "settings.resolution";
    private const string FullscreenKey = "settings.fullscreen";
    private const string QualityKey = "settings.quality";
    private const string VSyncKey = "settings.vsync";

    private const float MinVolume = 0.0001f;
    private const float DefaultVolume = 0.75f;
    private const float DecibelMultiplier = 20f;
    private const int Enabled = 1;
    private const int Disabled = 0;
    private const int VSyncOff = 0;
    private const int VSyncOn = 1;
    private const int TargetRefreshRate = 60;

    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [Header("Video")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle vSyncToggle;

    private Resolution[] resolutions;

    private void Start()
    {
        BuildResolutionOptions();
        BuildQualityOptions();
        LoadSettings();
    }

    private void BuildResolutionOptions()
    {
        if (resolutionDropdown == null)
            return;

        resolutions = FilterUniqueResolutions(Screen.resolutions);
        var labels = new List<string>(resolutions.Length);
        var current = 0;

        for (var i = 0; i < resolutions.Length; i++)
        {
            labels.Add($"{resolutions[i].width} x {resolutions[i].height}");
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                current = i;
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(labels);
        resolutionDropdown.SetValueWithoutNotify(current);
    }

    private static Resolution[] FilterUniqueResolutions(Resolution[] all)
    {
        var seen = new HashSet<string>();
        var unique = new List<Resolution>();

        foreach (var res in all)
        {
            var key = $"{res.width}x{res.height}";
            if (seen.Add(key))
                unique.Add(res);
        }

        return unique.ToArray();
    }

    private void BuildQualityOptions()
    {
        if (qualityDropdown == null)
            return;

        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new List<string>(QualitySettings.names));
        qualityDropdown.SetValueWithoutNotify(QualitySettings.GetQualityLevel());
    }

    private void LoadSettings()
    {
        SetSliderValue(masterVolumeSlider, PlayerPrefs.GetFloat(MasterVolumeKey, DefaultVolume));
        SetSliderValue(musicVolumeSlider, PlayerPrefs.GetFloat(MusicVolumeKey, DefaultVolume));
        SetSliderValue(sfxVolumeSlider, PlayerPrefs.GetFloat(SfxVolumeKey, DefaultVolume));

        ApplyVolume(MasterVolumeParam, PlayerPrefs.GetFloat(MasterVolumeKey, DefaultVolume));
        ApplyVolume(MusicVolumeParam, PlayerPrefs.GetFloat(MusicVolumeKey, DefaultVolume));
        ApplyVolume(SfxVolumeParam, PlayerPrefs.GetFloat(SfxVolumeKey, DefaultVolume));

        var fullscreen = PlayerPrefs.GetInt(FullscreenKey, Enabled) == Enabled;
        SetToggleValue(fullscreenToggle, fullscreen);
        Screen.fullScreen = fullscreen;

        var vSync = PlayerPrefs.GetInt(VSyncKey, Enabled) == Enabled;
        SetToggleValue(vSyncToggle, vSync);
        QualitySettings.vSyncCount = vSync ? VSyncOn : VSyncOff;

        if (qualityDropdown != null)
        {
            var quality = PlayerPrefs.GetInt(QualityKey, QualitySettings.GetQualityLevel());
            qualityDropdown.SetValueWithoutNotify(quality);
            QualitySettings.SetQualityLevel(quality);
        }

        if (resolutionDropdown != null && resolutions != null)
        {
            var index = PlayerPrefs.GetInt(ResolutionKey, resolutionDropdown.value);
            index = Mathf.Clamp(index, 0, resolutions.Length - 1);
            resolutionDropdown.SetValueWithoutNotify(index);
            ApplyResolution(index);
        }
    }

    public void SetMasterVolume(float value) => SaveVolume(MasterVolumeParam, MasterVolumeKey, value);

    public void SetMusicVolume(float value) => SaveVolume(MusicVolumeParam, MusicVolumeKey, value);

    public void SetSfxVolume(float value) => SaveVolume(SfxVolumeParam, SfxVolumeKey, value);

    private void SaveVolume(string parameter, string key, float value)
    {
        ApplyVolume(parameter, value);
        PlayerPrefs.SetFloat(key, value);
    }

    private void ApplyVolume(string parameter, float value)
    {
        if (audioMixer == null)
            return;

        var clamped = Mathf.Max(value, MinVolume);
        audioMixer.SetFloat(parameter, Mathf.Log10(clamped) * DecibelMultiplier);
    }

    public void SetFullscreen(bool value)
    {
        Screen.fullScreen = value;
        PlayerPrefs.SetInt(FullscreenKey, value ? Enabled : Disabled);
    }

    public void SetVSync(bool value)
    {
        QualitySettings.vSyncCount = value ? VSyncOn : VSyncOff;
        PlayerPrefs.SetInt(VSyncKey, value ? Enabled : Disabled);
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt(QualityKey, index);
    }

    public void SetResolution(int index)
    {
        ApplyResolution(index);
        PlayerPrefs.SetInt(ResolutionKey, index);
    }

    private void ApplyResolution(int index)
    {
        if (resolutions == null || index < 0 || index >= resolutions.Length)
            return;

        var res = resolutions[index];
        var rate = new RefreshRate { numerator = TargetRefreshRate, denominator = 1 };
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode, rate);
    }

    private static void SetSliderValue(Slider slider, float value)
    {
        if (slider != null)
            slider.SetValueWithoutNotify(value);
    }

    private static void SetToggleValue(Toggle toggle, bool value)
    {
        if (toggle != null)
            toggle.SetIsOnWithoutNotify(value);
    }
}
