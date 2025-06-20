using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings instance;

    public float mouseSensitivity = 5f;

    public float musicVolume = 100f;
    public float effectsVolume = 100f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(Settings.instance && Settings.instance != this)
        {
            Destroy(gameObject);
        }
        else if(!instance)
        {
            instance = this;
        }

        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSensitivity(float newSensitivity)
    {
        mouseSensitivity = newSensitivity;
        mouseSensitivity = Mathf.Clamp(mouseSensitivity, 1f, 100f);
    }

    public void SetMusicVolume(float newVolume)
    {
        musicVolume = newVolume;
        musicVolume = Mathf.Clamp(musicVolume, 0, 100);
    }

    public void SetEffectsVolume(float newVolume)
    {
        effectsVolume = newVolume;
        effectsVolume = Mathf.Clamp(musicVolume, 0, 100);
    }
}