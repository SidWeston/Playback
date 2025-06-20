using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;

    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject gameUI;

    [SerializeField] private GameObject eventSystem;
    private GameObject events;

    [SerializeField] private RawImage actionSymbol;

    [SerializeField] private Texture2D recordSymbol, pauseSymbol, playSymbol;
    public TextMeshProUGUI recordTime;

    public Button backButton;
    public Button backToMenuButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(GameUI.instance && GameUI.instance != this)
        {
            Destroy(gameObject);
        }
        else if(!GameUI.instance)
        {
            instance = this;
        }

        DontDestroyOnLoad(this);

        if(!events)
        {
            events = Instantiate(eventSystem, transform);
        }

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            canvas.gameObject.SetActive(false);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        InputManager.instance.pauseKey.keyPress += OnPause;

        SetPauseSymbol();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //probably bad practice, but in the build this will only be the menu scene
        if(scene.buildIndex == 0)
        {
            canvas.gameObject.SetActive(false);
            settingsMenu.SetActive(false);
            gameUI.SetActive(false);
            return;
        }
        else if(scene.buildIndex != 0 && !canvas.gameObject.activeSelf)
        {
            instance.canvas.gameObject.SetActive(true);
            settingsMenu.SetActive(false);
            gameUI.SetActive(true);
        }
        CancelRecordTime();
        SetPauseSymbol();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPause(bool input)
    {
        if(input)
        {
            if(!settingsMenu.activeSelf)
            {
                OpenPauseMenu();
            }
            else
            {
                ClosePauseMenu();
            } 
        }
    }

    public void OpenPauseMenu()
    {
        settingsMenu.SetActive(true);
        gameUI.SetActive(false);
    }

    public void ClosePauseMenu()
    {
        settingsMenu.SetActive(false);
        gameUI.SetActive(true);
    }

    public void SetRecordSymbol()
    {
        actionSymbol.texture = recordSymbol;
    }

    public void SetPauseSymbol()
    {
        actionSymbol.texture = pauseSymbol;
    }

    public void SetPlaySymbol()
    {
        actionSymbol.texture = playSymbol;
    }

    public void SetRecordTime(float time)
    {
        recordTime.text = (Mathf.Floor(time * 10) / 10).ToString();
    }

    public void CancelRecordTime()
    {
        recordTime.text = "N/A";
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}