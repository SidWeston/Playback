using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInput;

public class InputManager : MonoBehaviour, IPlayerActions
{
    //singleton setup
    public static InputManager instance;
    private PlayerInput input;

    public bool inputEnabled = true;

    //pspspsps this is persistent across game sessions, so it needs to not change
    private const string rebindsKey = "InputRebinds";

    //events and keys
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
        //create singleton data if its not there already
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        input = new PlayerInput();
        input.Player.SetCallbacks(this);
        input.Player.Enable();
        LoadRebinds();

        DontDestroyOnLoad(gameObject);
    }

    public void RebindAction(InputAction action, Action onComplete = null)
    {
        //make sure no classes are taking input whilst we're rebinding
        input.Player.Disable();

        //Start() means that the Unity InputSystem will listen for the next button pressed, and assign it to the action.
        action.PerformInteractiveRebinding().WithControlsExcluding("Mouse").OnMatchWaitForAnother(0.1f).OnComplete(callback =>
        {
            callback.Dispose();
            input.Player.Enable();
            onComplete?.Invoke();
            SaveRebinds();
        }).Start();
    }

    public void SaveRebinds()
    {
        string rebinds = input.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(rebindsKey, rebinds);
        PlayerPrefs.Save();
    }

    public void LoadRebinds()
    {
        if (PlayerPrefs.HasKey(rebindsKey))
        {
            string rebinds = PlayerPrefs.GetString(rebindsKey);
            input.LoadBindingOverridesFromJson(rebinds);
        }
    }

    public void ResetBindings()
    {
        input.RemoveAllBindingOverrides();
        PlayerPrefs.DeleteKey(rebindsKey);
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

        if (context.ReadValue<float>() > 0)
        {
            interactKey.KeyDown();           
        }
        else if (context.ReadValue<float>() <= 0)
        {
            interactKey.KeyUp();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!inputEnabled) return;

        if (context.ReadValue<float>() > 0)
        {
            jumpKey.KeyDown();
        }
        else if (context.ReadValue<float>() <= 0)
        {
            jumpKey.KeyUp();
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (!inputEnabled) return;

        if (context.ReadValue<float>() > 0)
        {
            crouchKey.KeyDown();
        }
        else if (context.ReadValue<float>() <= 0)
        {
            crouchKey.KeyUp();
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (!inputEnabled) return;

        if (context.ReadValue<float>() > 0)
        {
            sprintKey.KeyDown();
        }
        else if (context.ReadValue<float>() <= 0)
        {
            sprintKey.KeyUp();
        }
    }

    public void OnToggleGhost(InputAction.CallbackContext context)
    {
        if (!inputEnabled) return;

        if (context.ReadValue<float>() > 0)
        {
            ghostKey.KeyDown();
        }
        else if (context.ReadValue<float>() <= 0)
        {
            ghostKey.KeyUp();
        }
    }

    public void OnRecordGhost(InputAction.CallbackContext context)
    {
        if (!inputEnabled) return;

        if (context.ReadValue<float>() > 0)
        {
            recordKey.KeyDown();
        }
        else if (context.ReadValue<float>() <= 0)
        {
            recordKey.KeyUp();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
        {
            pauseKey.KeyDown();
        }
        else if (context.ReadValue<float>() <= 0)
        {
            pauseKey.KeyUp();
        }
    }

    public void OnRewind(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
        {
            rewindKey.KeyDown();
        }
        else if (context.ReadValue<float>() <= 0)
        {
            rewindKey.KeyUp();
        }
    }

    public void OnSelectOne(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
        {
            selectOne.KeyDown();
        }
        else if (context.ReadValue<float>() <= 0)
        {
            selectOne.KeyUp();
        }
    }

    public void OnSelectTwo(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
        {
            selectTwo.KeyDown();
        }
        else if (context.ReadValue<float>() <= 0)
        {
            selectTwo.KeyUp();
        }
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if(context.ReadValue<float>() > 0)
        {
            dropKey.KeyDown();
        }
        else if(context.ReadValue<float>() <= 0)
        {
            dropKey.KeyUp();
        }
    }
}

//basic custom class for handling inputs + getting around unity's weird bullshit with the new input system
public class InputKey
{
    public event Action<bool> keyPress;

    public bool isDown = false;    
    
    public void InvokeKeyPress(bool input)
    {
        //true for key down, false for key up
        keyPress?.Invoke(input);
    }

    public void KeyDown()
    {
        //check for isDown here, because otherwise the input system will register the key press twice
        //due to there being both a Started() and Performed() event for inputs.
        if (!isDown)
        {
            isDown = true;
            InvokeKeyPress(true);
        }
    }

    public void KeyUp()
    {
        isDown = false;
        InvokeKeyPress(false);
    }
}