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
    
    public event Action<bool> interactKey;    
    public event Action<bool> crouchKey;    
    public event Action<bool> sprintKey;    
    public event Action<bool> jumpKey;    
    public event Action<bool> recordKey;    
    public event Action<bool> ghostKey;    
    public event Action<bool> pauseKey;    
    public event Action<bool> selectOne;    
    public event Action<bool> selectTwo;    
    public event Action<bool> dropKey;    
    public event Action<bool> shootKey;   
    public event Action<bool> pauseGhostKey;
    public event Action<bool> rewindGhostKey;

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

        if (context.performed)
        {
            interactKey?.Invoke(true);       
        }
        else if (context.canceled)
        {
            interactKey?.Invoke(false);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!inputEnabled) return;

        if (context.performed)
        {
            jumpKey?.Invoke(true);
        }
        else if (context.canceled)
        {
            jumpKey?.Invoke(false);
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (!inputEnabled) return;

        if (context.performed)
        {
            crouchKey?.Invoke(true);
        }
        else if (context.canceled)
        {
            crouchKey?.Invoke(false);
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (!inputEnabled) return;

        if (context.performed)
        {
            sprintKey?.Invoke(true);
        }
        else if (context.canceled)
        {
            sprintKey?.Invoke(false);
        }
    }

    public void OnRecordGhost(InputAction.CallbackContext context)
    {
        if (!inputEnabled) return;

        if (context.performed)
        {
            recordKey?.Invoke(true);
        }
        else if (context.canceled)
        {
            recordKey?.Invoke(false);
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            pauseKey?.Invoke(true);
        }
        else if (context.canceled)
        {
            pauseKey?.Invoke(false);
        }
    }

    public void OnSelectOne(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            selectOne?.Invoke(true);
        }
        else if (context.canceled)
        {
            selectOne?.Invoke(false);
        }
    }

    public void OnSelectTwo(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            selectTwo?.Invoke(true);
        }
        else if (context.canceled)
        {
            selectTwo?.Invoke(false);
        }
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            dropKey?.Invoke(true);
        }
        else if (context.canceled)
        {
            dropKey?.Invoke(false);
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            shootKey?.Invoke(true);
        }
        else if (context.canceled)
        {
            shootKey?.Invoke(false);
        }
    }

    public void OnDestroyGhost(InputAction.CallbackContext context)
    {
        if (!inputEnabled) return;

        if (context.performed)
        {
            ghostKey?.Invoke(true);
        }
        else if (context.canceled)
        {
            ghostKey?.Invoke(false);
        }
    }

    public void OnPauseGhost(InputAction.CallbackContext context)
    {
        if (!inputEnabled) return;

        if (context.performed)
        {
            pauseGhostKey?.Invoke(true);
        }
        else if (context.canceled)
        {
            pauseGhostKey?.Invoke(false);
        }
    }

    public void OnRewindGhost(InputAction.CallbackContext context)
    {
        if (!inputEnabled) return;

        if (context.performed)
        {
            rewindGhostKey?.Invoke(true);
        }
        else if (context.canceled)
        {
            rewindGhostKey?.Invoke(false);
        }
    }
}