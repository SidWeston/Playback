using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject controlsMenu;
    public GameObject levelSelect;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void SettingsButton()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void SettingsBackButton()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void ControlsButton()
    {
        mainMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }
    
    public void ControlsBackButton()
    {
        mainMenu.SetActive(true);
        controlsMenu.SetActive(false);
    }

    public void LevelSelectButton()
    {
        mainMenu.SetActive(false);
        levelSelect.SetActive(true);
    }

    public void LevelSelectBack()
    {
        mainMenu.SetActive(true);
        levelSelect.SetActive(false);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}