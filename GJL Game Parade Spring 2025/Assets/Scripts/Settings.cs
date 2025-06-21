using System;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings instance;

    public float mouseSensitivity = 5f;

    public float musicVolume = 100f;
    public float effectsVolume = 100f;

    public event Action<float> effectsVolumeChange;
    public event Action<float> musicVolumeChange;

    private void Awake()
    {
        if (Settings.instance && Settings.instance != this)
        {
            Destroy(gameObject);
        }
        else if (!instance)
        {
            instance = this;
        }

        DontDestroyOnLoad(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSensitivity(float newSensitivity)
    {
        mouseSensitivity = newSensitivity;
    }

    public void SetMusicVolume(float newVolume)
    {
        musicVolume = newVolume;
        musicVolumeChange?.Invoke(musicVolume / 100);
    }

    public void SetEffectsVolume(float newVolume)
    {
        effectsVolume = newVolume;
        effectsVolumeChange?.Invoke(effectsVolume / 100);
    }
}