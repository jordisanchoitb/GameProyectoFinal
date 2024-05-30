using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControllerConfiguration : MonoBehaviour
{
    public const float DefaultAudio = 0.5f;
    // Variables audio
    public Slider sliderAudio;
    public float sliderValue;
    public Image imageMute;

    // Varibales fullscreen
    public Toggle toggleFullScreen;

    // Variables quality
    public TMP_Dropdown dropdownQuality;
    public int qualityIndex;

    private void Start()
    {
        sliderAudio.value = PlayerPrefs.GetFloat("volumeMusic", DefaultAudio);
        AudioListener.volume = sliderAudio.value;
        CheckMute();

        if (Screen.fullScreen)
        {
            toggleFullScreen.isOn = true;
        }
        else
        {
            toggleFullScreen.isOn = false;
        }
    }

    public void AdjustQuality()
    {
        QualitySettings.SetQualityLevel(dropdownQuality.value);
        PlayerPrefs.SetInt("qualityIndex", qualityIndex);
        qualityIndex = dropdownQuality.value;
    }

    public void ActivateFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }

    public void ChangeSlider(float valor)
    {
        sliderValue = valor;
        PlayerPrefs.SetFloat("volumeMusic", sliderValue);
        AudioListener.volume = sliderValue;
        CheckMute();
    }

    public void CheckMute()
    {
        if (sliderAudio.value == 0)
        {
            imageMute.enabled = true;
        }
        else
        {
            imageMute.enabled = false;
        }
    }
}
