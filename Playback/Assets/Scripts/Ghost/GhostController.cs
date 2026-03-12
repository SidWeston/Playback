using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    private GhostPlayer currentGhost;
    public List<GhostPlayer> ghosts; //keep it in a list so I can expand to more ghosts if necessary, but need to figure out input first.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.instance.ghostKey.keyPress += ToggleGhost;
        InputManager.instance.recordKey.keyPress += StartRecording;
        InputManager.instance.selectOne.keyPress += SelectGhostA;
        InputManager.instance.selectTwo.keyPress += SelectGhostB;
        InputManager.instance.interactKey.keyPress += OnInteract;        
        InputManager.instance.pauseGhostKey.keyPress += OnPause;
        InputManager.instance.rewindGhostKey.keyPress += OnRewind;

        //extra events to control crouching and sprinting anims on ghost
        //as they cancel each other, but dont change the anims if they are held at the same time
        TryGetComponent(out PlayerMovement movement);
        movement.crouchEvent += OnCrouch;
        movement.sprintEvent += OnSprint;        

        if (ghosts.Count == 1)
        {
            currentGhost = ghosts[0];
            GameUI.instance.SetGhostUIActive(0);
        }
    }

    private void SelectGhostA(bool input)
    {
        if(input && ghosts[0] != null)
        {
            currentGhost = ghosts[0];
            GameUI.instance.SetGhostUIActive(0);
        }
    }

    private void SelectGhostB(bool input)
    {
        if (input && ghosts[1] != null)
        {
            currentGhost = ghosts[1];
            GameUI.instance.SetGhostUIActive(1);
        }
    }

    private void ToggleGhost(bool input)
    {
        if(input && currentGhost != null)
        {
            if(currentGhost.ghostState == GhostState.Playing || currentGhost.ghostState == GhostState.Rewinding) currentGhost.DeactivateGhost();
        }
    }

    private void StartRecording(bool input)
    {
        if (input && currentGhost != null)
        {
            currentGhost.StartRecording(true);
        }
    }

    public bool IsRecording() //linked to the player rewind mechanic
    {
        if (currentGhost.ghostState == GhostState.Recording) return true;
        return false;
    }

    private void RecordEvent(GhostEvent.EventType type) //seperate function for potential events not caused by the player
    {
        currentGhost.RecordEvent(type);
    }

    private void OnInteract(bool input)
    { 
        if(input)
        {
            if(currentGhost && currentGhost.ghostState == GhostState.Recording)
            {
                RecordEvent(GhostEvent.EventType.Interact);
            }
        }
    }

    private void OnCrouch(bool input)
    {
        if(currentGhost && currentGhost.ghostState == GhostState.Recording)
        {
            if(input)
            {
                RecordEvent(GhostEvent.EventType.Crouch);
            }
            else
            {
                RecordEvent(GhostEvent.EventType.UnCrouch);
            }
        }
    }

    private void OnSprint(bool input)
    {
        if(currentGhost && currentGhost.ghostState == GhostState.Recording)
        {
            if(input)
            {
                RecordEvent(GhostEvent.EventType.Sprint);
            }
            else
            {
                RecordEvent(GhostEvent.EventType.UnSprint);
            }
        }
    }

    private void OnPause(bool input)
    {
        if (currentGhost.ghostState != GhostState.Playing && currentGhost.ghostState != GhostState.Paused && currentGhost.ghostState != GhostState.Rewinding) return;

        if(input && currentGhost)
        {
            currentGhost.TogglePause();
        }
    }

    private void OnRewind(bool input)
    {
        if (currentGhost.ghostState != GhostState.Playing && currentGhost.ghostState != GhostState.Rewinding) return;

        if(input && currentGhost)
        {
            currentGhost.ToggleRewind();
        }
    }
}