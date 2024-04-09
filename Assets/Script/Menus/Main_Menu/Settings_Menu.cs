using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings_Menu : MonoBehaviour
{
    //Player Prefs for both Vibration and PushAlarm
    private const string VibrationStateKey = "VibrationState";
    private const string PushAlarmStateKey = "PushAlarmState";

    //Player Prefs for Sound
    public const string SoundFxVolumeKey = "SoundFxVolume";
    public const string SoundTextKey = "SoundText";

    //Player Prefs for Music
    public const string MusicVolumeKey = "MusicVolume";
    public const string MusicTextKey = "MusicText";

    //GameObjects to set in the editor/
    public Button pushAlarmButton;
    public Slider soundFxSlider;
    public Slider musicSlider;
    public Button vibrationButton;
    public GameObject ChangePassword;

    [SerializeField] private AudioSource SliderSound;

    //Bools for the buttons to change the image.
    private bool isVibrationOn = false;
    private bool isPushAlarmOn = false;

    private void OnEnable()
    {
        ChangePassword.SetActive(false);
        LoadSettings();
    }

    public void OnSoundFxSliderChanged()
    {
        float SoundVolume = soundFxSlider.value; // Retrieve the value directly from the slider
        //Debug.Log("SoundVolume" + SoundVolume);

        //AudioManager.SetSoundFxVolume(SoundVolume);
        AudioManager.instance.SetSfxVolume(SoundVolume);
        SliderSound.Play();

        // Save the slider information
        PlayerPrefs.SetFloat(SoundFxVolumeKey, SoundVolume);
        PlayerPrefs.SetString(SoundTextKey, SoundVolume.ToString()); // Save the text value
        SaveSound();

        // Change the IconON image based on the slider value
        string imagePath = (Mathf.Approximately(SoundVolume, 0f)) ? "UI/SettingsMenu/icon_white_sound_mute" : "UI/SettingsMenu/icon_white_sound";

        // Change the IconON image
        Transform iconOnTransform = soundFxSlider.transform.parent.Find("IconOn");
        if (iconOnTransform != null)
        {
            Image imageComponent = iconOnTransform.GetComponent<Image>();
            Sprite loadedSprite = Resources.Load<Sprite>(imagePath);
            if (loadedSprite != null)
                imageComponent.sprite = loadedSprite;
        }

        // Change the SoundText to display the slider value
        TextMeshProUGUI soundTextComponent = soundFxSlider.transform.Find("SoundText")?.GetComponent<TextMeshProUGUI>();
        if (soundTextComponent != null)
            soundTextComponent.text = soundFxSlider.value.ToString();


    }

    public void OnMusicSliderChanged()
    {
        float MusicVolume = musicSlider.value; // Retrieve the value directly from the slider

        //AudioManager.SetMusicVolume(MusicVolume);
        AudioManager.instance.SetMusicVolume(MusicVolume);
        SliderSound.Play();

        // Save the slider information
        PlayerPrefs.SetFloat(MusicVolumeKey, MusicVolume);
        PlayerPrefs.SetString(MusicTextKey, MusicVolume.ToString()); // Save the text value
        SaveMusic();

        // Change the IconON image based on the slider value
        string imagePath = (Mathf.Approximately(MusicVolume, 0f)) ? "UI/SettingsMenu/icon_white_bgm_mute" : "UI/SettingsMenu/icon_white_bgm";

        // Change the IconON image
        Transform iconOnTransform = musicSlider.transform.parent.Find("IconOn");
        if (iconOnTransform != null)
        {
            Image imageComponent = iconOnTransform.GetComponent<Image>();
            Sprite loadedSprite = Resources.Load<Sprite>(imagePath);
            if (loadedSprite != null)
                imageComponent.sprite = loadedSprite;
        }

        // Change the SoundText to display the slider value
        TextMeshProUGUI soundTextComponent = musicSlider.transform.Find("MusicText")?.GetComponent<TextMeshProUGUI>();
        if (soundTextComponent != null)
            soundTextComponent.text = musicSlider.value.ToString();
    }

    public void OnPushAlarmButtonClicked()
    {
        // Toggle push alarm state
        isPushAlarmOn = !isPushAlarmOn;
        // Toggle push alarm state
        SetPushAlarmState(isPushAlarmOn);

        // Save settings
        SaveSound();
    }

    public void OnVibrationButtonClicked()
    {
        // Toggle vibration state
        isVibrationOn = !isVibrationOn;
        SetVibrationState(isVibrationOn);

        // Save settings
        SaveSound();
    }

    private void SetPushAlarmState(bool isPushAlarmOn)
    {
        // Move the handle based on push alarm state
        RectTransform handleRectTransform = pushAlarmButton.transform.Find("Toggle")?.GetComponent<RectTransform>();

        if (handleRectTransform != null)
        {
            // Move the handle based on push alarm state
            float newPositionX = isPushAlarmOn ? 120f : -20f;
            handleRectTransform.anchoredPosition = new Vector2(newPositionX, handleRectTransform.anchoredPosition.y);
        }

        // Change the image based on push alarm state
        string imagePath = isPushAlarmOn ? "UI/SettingsMenu/switch_on_frame" : "UI/SettingsMenu/switch_off_frame";
        string fullPath = "Assets/Resources/" + imagePath + ".png";

        // Change the sprite of the button based on the full path
        Image imageComponent = pushAlarmButton.GetComponent<Image>();
        imageComponent.sprite = Resources.Load<Sprite>(imagePath);

        // Update the TextMeshPro-Text component based on push alarm state
        TextMeshProUGUI textComponent = pushAlarmButton.transform.Find("Text_On")?.GetComponent<TextMeshProUGUI>();

        if (textComponent != null)
        {
            textComponent.text = isPushAlarmOn ? "ON" : "OFF";

            // Adjust alignment based on the state
            textComponent.alignment = isPushAlarmOn ? TextAlignmentOptions.Center : TextAlignmentOptions.MidlineRight;
        }
        if (isPushAlarmOn)
        {
            PushAlarmManager.PushAlarm = true;
        }
        else if (!isPushAlarmOn)
        {
            PushAlarmManager.PushAlarm = false;
        }
    }

    private void SetVibrationState(bool isVibrationOn)
    {
        // Update UI or handle visual changes for the vibration button here
        RectTransform handleRectTransform = vibrationButton.transform.Find("Toggle")?.GetComponent<RectTransform>();

        if (handleRectTransform != null)
        {
            // Move the handle based on vibration state
            float newPositionX = isVibrationOn ? 120f : -20f;
            handleRectTransform.anchoredPosition = new Vector2(newPositionX, handleRectTransform.anchoredPosition.y);
        }

        // Change the image based on vibration state
        string imagePath = isVibrationOn ? "UI/SettingsMenu/switch_on_frame" : "UI/SettingsMenu/switch_off_frame";
        string fullPath = "Assets/Resources/" + imagePath + ".png";

        // Change the sprite of the button based on the full path
        Image imageComponent = vibrationButton.GetComponent<Image>();
        imageComponent.sprite = Resources.Load<Sprite>(imagePath);

        // Update the TextMeshPro-Text component based on push alarm state
        TextMeshProUGUI textComponent = vibrationButton.transform.Find("Text_On")?.GetComponent<TextMeshProUGUI>();

        if (textComponent != null)
        {
            textComponent.text = isVibrationOn ? "ON" : "OFF";

            // Adjust alignment based on the state
            textComponent.alignment = isVibrationOn ? TextAlignmentOptions.Center : TextAlignmentOptions.MidlineRight;
        }
        if (isVibrationOn)
        {
            VibrationManager.Vibration = true;
        }
        else if (!isVibrationOn)
        {
            VibrationManager.Vibration = false;
        }
    }

    //Saves the sound
    private void SaveSound()
    {
        PlayerPrefs.SetInt(VibrationStateKey, isVibrationOn ? 1 : 0);
        PlayerPrefs.SetInt(PushAlarmStateKey, isPushAlarmOn ? 1 : 0);

        //Save the Sound Fx
        PlayerPrefs.SetFloat(SoundFxVolumeKey, soundFxSlider.value);
        PlayerPrefs.SetString(SoundTextKey, soundFxSlider.value.ToString()); // Save the text value
        PlayerPrefs.Save();
    }

    //Saves the Music
    private void SaveMusic()
    {
        //Save the Music
        PlayerPrefs.SetFloat(MusicVolumeKey, musicSlider.value);
        PlayerPrefs.SetString(MusicTextKey, musicSlider.value.ToString()); // Save the text value
        PlayerPrefs.Save();
    }

    //Loads everything.
    private void LoadSettings()
    {
        isVibrationOn = PlayerPrefs.GetInt(VibrationStateKey, 1) == 1;
        isPushAlarmOn = PlayerPrefs.GetInt(PushAlarmStateKey, 1) == 1;
        SetVibrationState(isVibrationOn);
        SetPushAlarmState(isPushAlarmOn);

        //Loading both the Sound Slider and text value.
        soundFxSlider.value = PlayerPrefs.GetFloat(SoundFxVolumeKey, 1.0f);
        TextMeshProUGUI soundTextComponent = soundFxSlider.transform.Find("SoundText")?.GetComponent<TextMeshProUGUI>();
        soundTextComponent.text = PlayerPrefs.GetString(SoundTextKey, "");

        //Loading both the Music Slider and text value.
        musicSlider.value = PlayerPrefs.GetFloat(MusicVolumeKey, 1.0f);
        TextMeshProUGUI musicTextComponent = musicSlider.transform.Find("MusicText")?.GetComponent<TextMeshProUGUI>();
        musicTextComponent.text = PlayerPrefs.GetString(MusicTextKey, "");
    }

    public void OpenDevPlan()
    {
        Application.OpenURL("https://trello.com/b/ZN8DAiXo/ift-game-studio-upcoming-news");
    }
}
