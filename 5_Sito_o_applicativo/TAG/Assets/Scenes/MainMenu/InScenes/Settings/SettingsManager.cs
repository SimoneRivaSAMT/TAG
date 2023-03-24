using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.PlayerPreferences;

public class SettingsManager : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider sensXSlider;
    public Slider sensYSlider;
    public TextMeshProUGUI volume;
    public TextMeshProUGUI sensX;
    public TextMeshProUGUI sensY;
    public TMP_Dropdown display;
    public TMP_Dropdown graphic;
    public float defVolume = 6;
    public float defSensX = 30;
    public float defSensY = 30;
    private void Awake()
    {
        display.value = PlayerPrefs.GetInt(PlayerPreference.SETTINGS_DISPLAY);
        graphic.value = PlayerPrefs.GetInt(PlayerPreference.SETTINGS_GRAPHICS);
    }

    void Start()
    {
        defVolume = PlayerPrefs.GetFloat(PlayerPreference.SETTINGS_VOLUME);
        defSensX = PlayerPrefs.GetFloat(PlayerPreference.SETTINGS_SENS_X);
        defSensY = PlayerPrefs.GetFloat(PlayerPreference.SETTINGS_SENS_Y);
        volumeSlider.value = defVolume;
        sensXSlider.value = defSensX;
        sensYSlider.value = defSensY;
    }

    void Update()
    {
        volume.text = defVolume.ToString();
        sensX.text = defSensX.ToString();
        sensY.text = defSensY.ToString();
        defVolume = volumeSlider.value;
        defSensX = sensXSlider.value;
        defSensY = sensYSlider.value;
        PlayerPrefs.SetFloat(PlayerPreference.SETTINGS_VOLUME, defVolume);
        PlayerPrefs.SetFloat(PlayerPreference.SETTINGS_SENS_X, defSensX);
        PlayerPrefs.SetFloat(PlayerPreference.SETTINGS_SENS_Y, defSensY);
        
    }

    public void DefaultSettings()
    {
        volumeSlider.value = 6;
        sensXSlider.value = 30;
        sensYSlider.value = 30;
        defVolume = volumeSlider.value;
        defSensX = sensXSlider.value;
        defSensY = sensYSlider.value;
    }

    public void DropdownValues()
    {
        PlayerPrefs.SetInt(PlayerPreference.SETTINGS_DISPLAY, display.value);
        PlayerPrefs.SetInt(PlayerPreference.SETTINGS_GRAPHICS, graphic.value);
        
    }
}
