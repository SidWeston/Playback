using UnityEngine;

public class GhostController : MonoBehaviour
{
    private GhostPlayer currentGhost;
    public GhostPlayer ghostA, ghostB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.instance.ghostKey.keyPress += ToggleGhost;
        InputManager.instance.recordKey.keyPress += StartRecording;
        InputManager.instance.selectOne.keyPress += SelectGhostA;
        InputManager.instance.selectTwo.keyPress += SelectGhostB;

        //ghostA = GameObject.FindGameObjectWithTag("Ghost A").GetComponent<GhostPlayer>();
        //ghostB = GameObject.FindGameObjectWithTag("Ghost B").GetComponent<GhostPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SelectGhostA(bool input)
    {
        if(input && ghostA != null)
        {
            currentGhost = ghostA;
            GameUI.instance.SetGhostUIActive(0);
        }
    }

    private void SelectGhostB(bool input)
    {
        if (input && ghostB != null)
        {
            currentGhost = ghostB;
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
}