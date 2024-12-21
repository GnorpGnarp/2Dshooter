using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] Slider soundSlider;
    [SerializeField] AudioMixer masterMixer;


    private void Start()
    {
        // Set initial volume from PlayerPrefs or default to 100 if not set
        float savedVolume = PlayerPrefs.GetFloat("SavedMasterVolume", 100f);
        SetVolume(savedVolume);
        soundSlider.value = savedVolume;
    }

    public void SetVolume(float value)
    {
        // Ensure the volume is within a valid range
        if (value < 1f) value = 0.0814f; // Logarithmic volume scale requires a minimal value
        else if (value > 100f) value = 100f; // Clamp to 100 to prevent values out of bounds

        // Set volume in AudioMixer
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(value / 100) * 20f);


        // Save the value to PlayerPrefs
        PlayerPrefs.SetFloat("SavedMasterVolume", value);
    }

    public void SetVolumeFromSlider()
    {
        // When slider value changes, update the volume
        SetVolume(soundSlider.value);
    }

    public void RefreshSlider(float value)
    {
        // Update slider UI with the current volume value
        soundSlider.value = value;
    }
}
