using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    //components
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform playerTransform;

    //settings
    [SerializeField] private float lookSensitivity;

    private float yRotation;

    public bool camEnabled = true;

    //input
    private Vector2 lookVector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.instance.lookEvent += OnLook;
        //InputManager.instance.pauseKey.keyPress += OnPause; //stops working if you go back and forth through the scenes, so its disabled and moved.

        if(GameUI.instance)
        {
            GameUI.instance.backButton.onClick.AddListener(HideCursor);
            GameUI.instance.backToMenuButton.onClick.AddListener(ShowCursor);
        }

        if (Cursor.lockState == CursorLockMode.None)
        {
            HideCursor();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!camEnabled) return;

        lookVector *= Settings.instance != null ? (Settings.instance.mouseSensitivity * 3) * Time.deltaTime : 100 * Time.deltaTime;
        yRotation -= lookVector.y;
        yRotation = Mathf.Clamp(yRotation, -90, 90);
        playerCamera.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        playerTransform.Rotate(Vector3.up * lookVector.x);
    }

    private void OnLook(Vector2 input)
    {
        lookVector = input;
    }

    private void OnPause(bool input)
    {
        if(input)
        {
            if(Cursor.lockState == CursorLockMode.Locked)
            {
                ShowCursor();
            }
            else
            {
                HideCursor();
            }
        }
    }

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        camEnabled = false;
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        camEnabled = true;
    }
}