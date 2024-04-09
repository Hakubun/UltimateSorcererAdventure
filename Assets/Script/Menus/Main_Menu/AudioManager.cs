using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioMixer Mixer;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitializeVolumes();
    }
    // Use this method to set the initial volumes based on saved settings
    public void InitializeVolumes()
    {
        SetMusicVolume(PlayerPrefs.GetFloat(Settings_Menu.MusicVolumeKey, 1.0f));
        SetSfxVolume(PlayerPrefs.GetFloat(Settings_Menu.SoundFxVolumeKey, 1.0f));


    }

    public void SetMusicVolume(float _vol)
    {
        if (_vol <= 0)
        {
            Mixer.SetFloat("MusicVolume", -80f);
        }
        else
        {
            _vol *= 0.1f;
            Mixer.SetFloat("MusicVolume", Mathf.Log10(_vol) * 20);
        }


    }

    public void SetSfxVolume(float _vol)
    {
        if (_vol <= 0)
        {
            Mixer.SetFloat("SFXVolume", -80f);
        }
        else
        {
            _vol *= 0.1f;
            Mixer.SetFloat("SFXVolume", Mathf.Log10(_vol) * 20);

        }
    }
}
