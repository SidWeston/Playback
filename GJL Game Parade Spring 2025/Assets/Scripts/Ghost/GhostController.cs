using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    private GhostPlayer currentGhost;
    public List<GhostPlayer> ghosts;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.instance.ghostKey.keyPress += ToggleGhost;
        InputManager.instance.recordKey.keyPress += StartRecording;
        InputManager.instance.selectOne.keyPress += SelectGhostA;
        InputManager.instance.selectTwo.keyPress += SelectGhostB;

        if (ghosts.Count == 1)
        {
            currentGhost = ghosts[0];
            GameUI.instance.SetGhostUIActive(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
            currentGhost.ToggleGhost(true);
        }
    }

    private void StartRecording(bool input)
    {
        if (input && currentGhost != null)
        {
            currentGhost.StartRecording(true);
        }
    }

    public void StopRecording()
    {
        currentGhost.StartRecording(true);
        currentGhost.ToggleGhost(true);
    }

    public bool IsRecording()
    {
        if (currentGhost.isRecording) return true;
        return false;
    }
}