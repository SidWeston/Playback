using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")]
    public float swayAmount = 0.02f;
    public float maxSwayAmount = 0.06f;
    public float smoothAmount = 6f;

    [Header("Bobbing Settings")]
    public float bobFrequency = 3f;
    public float bobAmplitude = 0.05f;
    public float bobSpeedMultiplier = 1f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float weaponSwayMulti = 1.0f;

    [SerializeField] private Transform weaponTransform;

    private Vector2 mouseInput;
    private Vector2 moveInput;
    private float bobTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = weaponTransform.localPosition;
        initialRotation = weaponTransform.localRotation;

        InputManager.instance.lookEvent += OnMouseMove;
        InputManager.instance.moveEvent += OnMove;
        InputManager.instance.sprintKey += OnSprint;
        InputManager.instance.shootKey += OnShoot;
    }

    private void OnDestroy()
    {
        InputManager.instance.lookEvent -= OnMouseMove;
        InputManager.instance.moveEvent -= OnMove;
        InputManager.instance.sprintKey -= OnSprint;
        InputManager.instance.shootKey -= OnShoot;
    }

    // Update is called once per frame
    void Update()
    {
        //calculate sway
        float movementX = -mouseInput.x * swayAmount * weaponSwayMulti;
        float movementY = -mouseInput.y * swayAmount * weaponSwayMulti;

        movementX = Mathf.Clamp(movementX, -maxSwayAmount, maxSwayAmount);
        movementY = Mathf.Clamp(movementY, -maxSwayAmount, maxSwayAmount);

        Vector3 targetPosition = new Vector3(movementX, movementY, 0f) + initialPosition;
        Quaternion targetRotation = Quaternion.Euler(initialRotation.eulerAngles.x + movementY * 30f,
            initialRotation.eulerAngles.y + movementX * 30f, initialRotation.eulerAngles.z);

        //add bobbing
        float moveMagnitude = moveInput.magnitude;
        if (moveMagnitude > 0.1f)
        {
            bobTimer += Time.deltaTime * bobFrequency * (moveMagnitude * bobSpeedMultiplier);
            float bobX = Mathf.Sin(bobTimer) * bobAmplitude * 0.5f;
            float bobY = Mathf.Cos(bobTimer * 2f) * bobAmplitude; //move faster vertically
            targetPosition += new Vector3(bobX, Mathf.Abs(bobY), 0f);
        }
        else
        {
            bobTimer = Mathf.Lerp(bobTimer, 0f, Time.deltaTime * 5f);
        }

        //apply
        weaponTransform.localPosition = Vector3.Lerp(weaponTransform.localPosition, targetPosition, Time.deltaTime * smoothAmount);
        weaponTransform.localRotation = Quaternion.Lerp(weaponTransform.localRotation, targetRotation, Time.deltaTime * smoothAmount);
    }

    private void OnMouseMove(Vector2 input)
    {
        mouseInput = input;
    }

    private void OnMove(Vector2 input)
    {
        moveInput = input;
    }

    private void OnSprint(bool input)
    {
        if(input)
        {
            bobFrequency *= 2;
        }
        else
        {
            bobFrequency /= 2;
        }
    }

    private void OnShoot(bool input)
    {
        if(input)
        {
            weaponTransform.localPosition += new Vector3(0, 0, -0.1f);
        }
    }
}
