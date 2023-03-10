using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        display.value = PlayerPrefs.GetInt("Display");
        graphic.value = PlayerPrefs.GetInt("Graphic");
    }

    // Start is called before the first frame update
    void Start()
    {

        defVolume = PlayerPrefs.GetFloat("Volume");
        defSensX = PlayerPrefs.GetFloat("SensX");
        defSensY = PlayerPrefs.GetFloat("SensY");
        volumeSlider.value = defVolume;
        sensXSlider.value = defSensX;
        sensYSlider.value = defSensY;
        Debug.Log(PlayerPrefs.GetInt("Display") + " " + PlayerPrefs.GetInt("Graphic"));
    }

    // Update is called once per frame
    void Update()
    {
        volume.text = defVolume.ToString();
        sensX.text = defSensX.ToString();
        sensY.text = defSensY.ToString();
        defVolume = volumeSlider.value;
        defSensX = sensXSlider.value;
        defSensY = sensYSlider.value;
        PlayerPrefs.SetFloat("Volume", defVolume);
        PlayerPrefs.SetFloat("SensX", defSensX);
        PlayerPrefs.SetFloat("SensY", defSensY);
        
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
        PlayerPrefs.SetInt("Display", display.value);
        PlayerPrefs.SetInt("Graphic", graphic.value);
        
    }
}
