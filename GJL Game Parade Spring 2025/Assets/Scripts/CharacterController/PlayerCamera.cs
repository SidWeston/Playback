using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    //components
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform playerTransform;

    //settings
    [SerializeField] private float lookSensitivity;

    private float yRotation;

    //input
    private Vector2 lookVector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.instance.lookEvent += OnLook;
        if(Settings.instance != null)
        {
            lookSensitivity = Settings.instance.mouseSensitivity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        lookVector *= lookSensitivity * Time.deltaTime;
        yRotation -= lookVector.y;
        yRotation = Mathf.Clamp(yRotation, -90, 90);
        playerCamera.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        playerTransform.Rotate(Vector3.up * lookVector.x);
    }

    private void OnLook(Vector2 input)
    {
        lookVector = input;
    }
}