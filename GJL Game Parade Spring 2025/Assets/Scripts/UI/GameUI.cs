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

    [SerializeField] private AudioSource musicSource;

    private PlayerCamera playerCam;

    private void Awake()
    {
        if (GameUI.instance && GameUI.instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!GameUI.instance)
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
        Settings.instance.musicVolumeChange += SetMusicVolume;

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
            musicSource.gameObject.SetActive(true);
            musicSource.Play();
        }

        //moved disabling the camera for the pause menu in here because for some reason if you play a level,
        //go back to menu and then go into another level it stops working. i think its something to do with
        //the event system but I can't figure it out right now, and I want to get this sumbitted on time for the jam.
        if(!playerCam && scene.buildIndex != 0)
        {
            playerCam = GameObject.FindGameObjectWithTag("Camera").GetComponent<PlayerCamera>();
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
        }
    }

    public void OpenPauseMenu()
    {
        settingsMenu.SetActive(true);
        gameUI.SetActive(false);

        playerCam.camDisabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ClosePauseMenu()
    {
        settingsMenu.SetActive(false);
        gameUI.SetActive(true);

        playerCam.camDisabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        ClosePauseMenu();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
}