using System.Collections.Generic;
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

    [SerializeField] private Texture2D recordSymbol, pauseSymbol, playSymbol;

    public List<GhostUI> ghostUIs = new List<GhostUI>();

    public Button backButton;
    public Button backToMenuButton;

    [SerializeField] private AudioSource musicSource;

    private PlayerCamera playerCam;

    [SerializeField] private bool testScene = false;

    [SerializeField] private RawImage emptySymbolPrefab;
    [SerializeField] private TextMeshProUGUI emptyTextPrefab;

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

        //currentActionSymbol = actionSymbolA;

        if(testScene)
        {
            SetupGhostUI();
        }

        for(int i = 0; i < ghostUIs.Count; i++)
        {
            UpdateGhostUIState(i, RecordState.Pause);
            UpdateGhostUITime(i, 0);
        }
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

        //need to ensure ghost ui is properly destroyed when changing from a previous level, or this will layer on top
        SetupGhostUI();

        for (int i = 0; i < ghostUIs.Count; i++)
        {
            UpdateGhostUIState(i, RecordState.Pause);
            UpdateGhostUITime(i, 0);
        }
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

        playerCam.camEnabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ClosePauseMenu()
    {
        settingsMenu.SetActive(false);
        gameUI.SetActive(true);

        playerCam.camEnabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UpdateGhostUIState(int index, RecordState state)
    {
        GhostUI ui = ghostUIs[index];

        switch (state)
        {
            case RecordState.Pause:
                {
                    ui.stateIcon.texture = pauseSymbol;
                    break;
                }
            case RecordState.Recording:
                {
                    ui.stateIcon.texture = recordSymbol;
                    break;
                }
            case RecordState.Play:
                {
                    ui.stateIcon.texture = playSymbol;
                    break;
                }
        }
    }

    public void UpdateGhostUITime(int index, float time)
    {
        GhostUI ui = ghostUIs[index];
        //adjust time to 2 decimal places
        float adjustedTime = (Mathf.Floor(time * 10) / 10);
        ui.timerText.text = adjustedTime.ToString();
    }

    public void SetGhostUIActive(int index)
    {
        for(int i = 0; i < ghostUIs.Count; i++)
        {
            if(index == i)
            {
                ghostUIs[i].stateIcon.material.SetFloat("_GhostBlend", 0);
                ghostUIs[i].stateIcon.material.SetFloat("_OutlineAlpha", 1);
                ghostUIs[i].stateIcon.material.SetFloat("_OutlinePixelWidth", 1f);
                ghostUIs[i].stateIcon.material.SetFloat("_ShineGlow", 0.2f);
            }
            else
            {
                ghostUIs[i].stateIcon.material.SetFloat("_GhostBlend", 1);
                ghostUIs[i].stateIcon.material.SetFloat("_OutlineAlpha", 0);
                ghostUIs[i].stateIcon.material.SetFloat("_OutlinePixelWidth", 0);
                ghostUIs[i].stateIcon.material.SetFloat("_ShineGlow", 0);
            }
        }
    }

    public void SetupGhostUI()
    {
        //find all ghosts
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject ghost in ghosts)
        {
            ghostUIs.Add(ghost.GetComponent<GhostUI>());
        }

        if (ghostUIs.Count == 0) return;

        float offset = 0;

        if(ghostUIs.Count > 1)
        {
            offset = 200 - (400 * (ghostUIs.Count - 1));
        }

        for(int i = 0; i < ghostUIs.Count; i++)
        {
            ghostUIs[i].stateIcon = Instantiate(emptySymbolPrefab, canvas.transform);
            ghostUIs[i].stateIcon.rectTransform.localScale = new Vector3(2, 2, 1);
            ghostUIs[i].stateIcon.rectTransform.localPosition = new Vector2(offset, -210);
            ghostUIs[i].stateIcon.material = new Material(ghostUIs[i].stateIcon.material);


            ghostUIs[i].timerText = Instantiate(emptyTextPrefab, canvas.transform);
            ghostUIs[i].timerText.rectTransform.localScale = new Vector3(2, 2, 1);
            ghostUIs[i].timerText.rectTransform.localPosition = new Vector3(offset, -360, 0);

            offset += 400;
        }
    }

    public void DestroyGhostUI()
    {
        for(int i = 0; i < ghostUIs.Count; i++)
        {
            Destroy(ghostUIs[i].stateIcon.gameObject);
            Destroy(ghostUIs[i].timerText.gameObject);
            ghostUIs.Remove(ghostUIs[i]);
        }
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

public enum RecordState
{ 
    Pause,
    Recording,
    Play
}