using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInput;

public class InputManager : MonoBehaviour, IPlayerActions
{
    public static InputManager instance;
    private PlayerInput input;

    public event Action<Vector2> moveEvent;
    public event Action<Vector2> lookEvent;

    public InputKey interactKey = new InputKey();
    public InputKey crouchKey = new InputKey();
    public InputKey sprintKey = new InputKey();
    public InputKey jumpKey = new InputKey();

    private void Awake()
    {
        if(input == null)
        {
            input = new PlayerInput();
            input.Player.SetCallbacks(this);
        }

        input.Player.Enable();

        if(instance == null)
        {
            instance = this;
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        moveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0 && interactKey.downCounter == 0)
        {
            interactKey.downCounter++;
            interactKey.InvokeKeyPress(true);
        }
        else if (context.ReadValue<float>() <= 0)
        {
            interactKey.downCounter = 0;
            interactKey.InvokeKeyPress(false);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0 && jumpKey.downCounter == 0)
        {
            jumpKey.downCounter++;
            jumpKey.InvokeKeyPress(true);
        }
        else if (context.ReadValue<float>() <= 0)
        {
            jumpKey.downCounter = 0;
            interactKey.InvokeKeyPress(false);
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0 && crouchKey.downCounter == 0)
        {
            crouchKey.downCounter++;
            crouchKey.InvokeKeyPress(true);
        }
        else if (context.ReadValue<float>() <= 0)
        {
            crouchKey.downCounter = 0;
            crouchKey.InvokeKeyPress(false);
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0 && sprintKey.downCounter == 0)
        {
            sprintKey.downCounter++;
            sprintKey.InvokeKeyPress(true);
        }
        else if (context.ReadValue<float>() <= 0)
        {
            sprintKey.downCounter = 0;
            sprintKey.InvokeKeyPress(false);
        }
    }
}

public class InputKey
{
    public event Action<bool> keyPress;

    public int downCounter = 0;
    public float lastPressTime = 0;
    public bool isDown = false;
    public bool doublePressed = false;
    
    public void Init()
    {
        downCounter = 0;
        lastPressTime = 0;
    }
    
    public void InvokeKeyPress(bool input)
    {
        isDown = input;
        keyPress?.Invoke(input);
    }
}