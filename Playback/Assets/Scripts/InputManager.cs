using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInput;

public class InputManager : MonoBehaviour, IPlayerActions
{
    public static InputManager instance;
    private PlayerInput input;

    public bool inputEnabled = true;

    public event Action<Vector2> moveEvent;
    public event Action<Vector2> lookEvent;

    public InputKey interactKey = new InputKey();
    public InputKey crouchKey = new InputKey();
    public InputKey sprintKey = new InputKey();
    public InputKey jumpKey = new InputKey();
    public InputKey recordKey = new InputKey();
    public InputKey ghostKey = new InputKey();
    public InputKey pauseKey = new InputKey();
    public InputKey rewindKey = new InputKey();
    public InputKey selectOne = new InputKey();
    public InputKey selectTwo = new InputKey();
    public InputKey dropKey = new InputKey();

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
        if (!inputEnabled) return;

        moveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (!inputEnabled) return;

        lookEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!inputEnabled) return;

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
        if (!inputEnabled) return;

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
        if (!inputEnabled) return;

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
        if (!inputEnabled) return;

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

    public void OnToggleGhost(InputAction.CallbackContext context)
    {
        if (!inputEnabled) return;

        if (context.ReadValue<float>() > 0 && ghostKey.downCounter == 0)
        {
            ghostKey.downCounter++;
            ghostKey.InvokeKeyPress(true);
        }
        else if (context.ReadValue<float>() <= 0)
        {
            ghostKey.downCounter = 0;
            ghostKey.InvokeKeyPress(false);
        }
    }

    public void OnRecordGhost(InputAction.CallbackContext context)
    {
        if (!inputEnabled) return;

        if (context.ReadValue<float>() > 0 && recordKey.downCounter == 0)
        {
            recordKey.downCounter++;
            recordKey.InvokeKeyPress(true);
        }
        else if (context.ReadValue<float>() <= 0)
        {
            recordKey.downCounter = 0;
            recordKey.InvokeKeyPress(false);
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0 && pauseKey.downCounter == 0)
        {
            pauseKey.downCounter++;
            pauseKey.InvokeKeyPress(true);
        }
        else if (context.ReadValue<float>() <= 0)
        {
            pauseKey.downCounter = 0;
            pauseKey.InvokeKeyPress(false);
        }
    }

    public void OnRewind(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0 && rewindKey.downCounter == 0)
        {
            rewindKey.downCounter++;
            rewindKey.InvokeKeyPress(true);
        }
        else if (context.ReadValue<float>() <= 0)
        {
            rewindKey.downCounter = 0;
            rewindKey.InvokeKeyPress(false);
        }
    }

    public void OnSelectOne(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0 && selectOne.downCounter == 0)
        {
            selectOne.downCounter++;
            selectOne.InvokeKeyPress(true);
        }
        else if (context.ReadValue<float>() <= 0)
        {
            selectOne.downCounter = 0;
            selectOne.InvokeKeyPress(false);
        }
    }

    public void OnSelectTwo(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0 && selectTwo.downCounter == 0)
        {
            selectTwo.downCounter++;
            selectTwo.InvokeKeyPress(true);
        }
        else if (context.ReadValue<float>() <= 0)
        {
            selectTwo.downCounter = 0;
            selectTwo.InvokeKeyPress(false);
        }
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if(context.ReadValue<float>() > 0 && dropKey.downCounter == 0)
        {
            dropKey.downCounter++;
            dropKey.InvokeKeyPress(true);
        }
        else if(context.ReadValue<float>() <= 0)
        {
            dropKey.downCounter = 0;
            dropKey.InvokeKeyPress(false);
        }
    }
}


//to anyone reading this - 
//I made this custom class for inputs because when a button is pressed, Unity's input system records an input twice when it presses.
//im fairly sure this is because it has two events for a button press: "started" and "performed" (and some for when its released, but those havent caused a problem)
//setting the input system up properly with these actions will solve it will almost definitely solve this problem,
//but I cant be bothered to do that, and this works just as well for my purposes, even if its likely a bit wasteful. 
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