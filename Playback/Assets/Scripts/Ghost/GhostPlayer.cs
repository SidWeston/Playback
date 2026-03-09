using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlayer : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    private CharacterController playerCharacterController;

    private List<GhostFrame> recording = new List<GhostFrame>();
    private List<GhostEvent> eventLog = new List<GhostEvent>();

    public GhostState ghostState;

    [SerializeField] private float frameInterval = 0.1f;
    [SerializeField] private float recordDuration = 5f;

    [SerializeField] private GhostAnimationController animationController;
    [SerializeField] private BoxCollider ghostCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayers;
    private bool stateSwapped = false;

    [SerializeField] private GameObject head, body;
    private Renderer headRenderer, bodyRenderer;
    [SerializeField] private Material ghostMat;
    private bool visualsActive;

    [SerializeField] private GhostUI ghostUI;

    private int currentFrameIndex = 0;
    private int currentEventIndex = 0;
    private float frameTimer = 0f;
    private float duration = 0f, fullDuration = 0f;
    private float recordingStartTime = 0f;
    private float playbackStartTime = 0f;
    
    [SerializeField] private Vector3 crouchedColSize;
    private Vector3 standingColSize;
    [SerializeField] private float crouchedColOffset = -0.3f;

    private bool playerOverlapping = false;
    private Collider[] overlapBuffer = new Collider[4];

    private bool collisionsDisabled = false;
    private bool earlyStop = false;

    private float recordDelay = 0.2f;
    private bool canRecord = true;

    private AudioSource glitchSound;
    [SerializeField] private float restartGlitchValue = 5f; //how much glitch happens when the recording begins a loop
    [SerializeField] private float baseGlitchValue = 0.1f; //how much glitch happens regularly in the middle of a recording

    //interaction stuff
    [SerializeField] private LayerMask interactableLayers;
    private const float interactRadius = 0.25f;
    private const float interactDistance = 5f;

    private bool paused = false;
    private bool rewind = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!playerMovement)
        {
            GameObject.FindGameObjectWithTag("Player").TryGetComponent(out PlayerMovement movement);
            playerMovement = movement;
        }

        headRenderer = head.GetComponent<Renderer>();
        bodyRenderer = body.GetComponent<Renderer>();
        ResetGlitch(); //ensure glitch effect isnt set to high by default
        ghostState = GhostState.Inactive; //assume ghost starts turned off

        ghostUI = GetComponent<GhostUI>();

        glitchSound = GetComponent<AudioSource>();
        Settings.instance.effectsVolumeChange += SetAudioVolume;

        standingColSize = ghostCollider.size;
        crouchedColSize = new Vector3(ghostCollider.size.x, ghostCollider.size.y / 2, ghostCollider.size.z);

        playerCharacterController = playerMovement.gameObject.GetComponent<CharacterController>();
    }
    #region Playback
    // Update is called once per frame
    void Update()
    {
        if (ghostState != GhostState.Playing || recording.Count < 2) return;
        
        frameTimer += Time.deltaTime;
        duration -= Time.deltaTime;

        GameUI.instance.UpdateGhostUITime(ghostUI.index, duration);

        while (frameTimer > frameInterval)
        {
            frameTimer -= frameInterval;
            currentFrameIndex++;
            //reached the end of playback, loop back to the start
            if (currentFrameIndex >= recording.Count - 1)
            {
                ApplyInitialFrameState();
                currentFrameIndex = 0;
                currentEventIndex = 0;
                playbackStartTime = Time.time;
                PerformGlitchEffect();
                glitchSound.Play();
                duration = fullDuration;

                CheckForPlayerOverlap();
            }
        }

        //get 2 frames to interpolate between
        GhostFrame a = recording[currentFrameIndex];
        GhostFrame b = recording[currentFrameIndex + 1];       
        float t = frameTimer / frameInterval;

        Vector3 position = Vector3.Lerp(a.position, b.position, t);
        Quaternion rotation = Quaternion.Slerp(a.rotation, b.rotation, t);
        transform.position = position;
        transform.rotation = rotation;

        float playbackTime = Time.time - playbackStartTime;

        while (currentEventIndex < eventLog.Count && eventLog[currentEventIndex].time <= playbackTime)
        {
            TriggerEvent(eventLog[currentEventIndex]);
            currentEventIndex++;
        }

        //if the movement direction changes, update the animation
        if(a.movementInput != b.movementInput || stateSwapped)
        {
            stateSwapped = false;
            animationController.PlayMovementAnimation(b.movementInput);
        }        
    }

    private void ApplyInitialFrameState()
    {
        GhostFrame firstFrame = recording[0];

        if(firstFrame.isCrouching)
        {            
            ghostCollider.size = crouchedColSize;
            ghostCollider.center = new Vector3(0, crouchedColOffset, 0);
            animationController.SwitchAnimSet(MoveMode.CROUCH);
            stateSwapped = true;
        }
        else if(firstFrame.isSprinting)
        {
            ghostCollider.size = standingColSize;
            ghostCollider.center = Vector3.zero;
            animationController.SwitchAnimSet(MoveMode.SPRINT);
            stateSwapped = true;
        }
        else
        {
            ghostCollider.size = standingColSize;
            ghostCollider.center = Vector3.zero;
            animationController.SwitchAnimSet(MoveMode.WALK);
            stateSwapped = true;
        }
    }

    #endregion

    #region Collision Handling
    private void FixedUpdate()
    {
        //stops the ghost running the player into a wall and clipping them through it
        CheckForPlayerWallCollision();

        //once the player is overlapping, it only needs to be checked for when they stop overlapping. 
        //this gets assigned at the start of each playback loop, as this is the primary time when overlaps and collision errors occur
        if (playerOverlapping)
        {
            CheckForPlayerOverlap();
        }
    }

    public void CheckForPlayerOverlap()
    {        
        //check for overlaps, with a slightly smaller collider to allow for some clearance
        int count = Physics.OverlapBoxNonAlloc(
            ghostCollider.bounds.center,
            ghostCollider.bounds.extents * 0.8f,
            overlapBuffer,
            ghostCollider.transform.rotation,
            playerLayer
        );
        
        //overlapping
        if(count > 0)
        {
            playerOverlapping = true;
            Physics.IgnoreCollision(ghostCollider, playerCharacterController, true);
        }
        else if(playerOverlapping) //not  
        {
            playerOverlapping = false;  
            Physics.IgnoreCollision(ghostCollider, playerCharacterController, false);
        }
    }

    private void OnDrawGizmos()
    {
        //visual debugging for the collision checks
        Gizmos.DrawCube(ghostCollider.bounds.center, ghostCollider.bounds.extents * 0.8f);
    }

    public void CheckForPlayerWallCollision()
    {
        //just to check if there is a wall on the other side of the player compared to the ghost
        //i.e is the player currently between the ghost and a wall and within a distance threshold
        Vector3 toPlayer = playerMovement.transform.position - transform.position;
        Ray rayToPlayer = new Ray(transform.position, toPlayer);
        if(!collisionsDisabled)
        {
            if (Physics.Raycast(rayToPlayer, out RaycastHit playerHit, 1.5f, playerLayer))
            {
                Ray rayThroughPlayer = new Ray(playerMovement.transform.position, toPlayer);
                if (Physics.Raycast(rayThroughPlayer, out RaycastHit wallHit, 1.5f, obstacleLayers))
                {
                    collisionsDisabled = true;
                    Physics.IgnoreCollision(ghostCollider, playerMovement.gameObject.GetComponent<CharacterController>(), true);
                }
            }
        }
        else if(collisionsDisabled && Vector3.Distance(transform.position, playerMovement.transform.position) > 1.25f)
        {
            collisionsDisabled = false;
            Physics.IgnoreCollision(ghostCollider, playerMovement.gameObject.GetComponent<CharacterController>(), false);
        }
        
    }

    private IEnumerator EnableColliderAfterFrame()
    {
        yield return new WaitForFixedUpdate();
        ghostCollider.enabled = !ghostCollider.enabled;
    }

    #endregion

    #region Recording
    public IEnumerator RecordGhostFrames()
    {
        //setup
        ghostState = GhostState.Recording;        
        earlyStop = false;
        if(visualsActive) ToggleGhost(true);
        List<GhostFrame> newFrames = new List<GhostFrame>();
        float timer = 0f;
        recordingStartTime = Time.time;

        GameUI.instance.UpdateGhostUIState(ghostUI.index, RecordState.Recording);

        //loop through
        while (timer < recordDuration && !earlyStop)
        {
            newFrames.Add(playerMovement.RecordFrame());
            yield return new WaitForSeconds(frameInterval);
            timer += frameInterval;
            GameUI.instance.UpdateGhostUITime(ghostUI.index, timer);
        }

        //finish
        fullDuration = timer;
        duration = timer;

        //toggle the ghost visuals and effects
        if (!visualsActive) ToggleGhost(true);
        recording = newFrames;
        currentFrameIndex = 0;
        PerformGlitchEffect();
        glitchSound.Play();

        //pass data to the UI handler
        GameUI.instance.UpdateGhostUIState(ghostUI.index, RecordState.Play);

        //setup to play the first frame of the recording
        frameTimer = 0;
        transform.position = recording[0].position;
        transform.rotation = recording[0].rotation;
        ghostState = GhostState.Playing;
        ApplyInitialFrameState();

        //check if the ghost has spawned in the same space as the player
        CheckForPlayerOverlap();

        //event stuff
        playbackStartTime = Time.time;
        currentEventIndex = 0;
    }

    public void RecordEvent(GhostEvent.EventType type)
    {
        eventLog.Add(new GhostEvent
        {
            time = Time.time - recordingStartTime,
            type = type
        });
    }

    public void StartRecording(bool input)
    {
        if (input && canRecord)
        {
            if (ghostState != GhostState.Recording) //start recording
            {
                if(ghostState == GhostState.Playing)
                {
                    ToggleGhost(true);
                }
                canRecord = false;
                Invoke(nameof(ResetCanRecord), recordDelay); //to stop recordings being spammed there is a short delay between activation and deactivation
                StartCoroutine(RecordGhostFrames());
            }
            else if (ghostState == GhostState.Recording) //end recording early
            {
                earlyStop = true;
            }
        }
    }

    public void StopRecording()
    {
        earlyStop = true;
    }
    #endregion

    private void TriggerEvent(GhostEvent ghostEvent)
    {
        //add more events when needed, for now just interaction
        switch (ghostEvent.type)
        {
            case GhostEvent.EventType.Interact:
                {
                    TryInteract();
                    break;
                }
            case GhostEvent.EventType.Crouch:
                {
                    ghostCollider.size = crouchedColSize;
                    ghostCollider.center = new Vector3(0, crouchedColOffset, 0);
                    animationController.SwitchAnimSet(MoveMode.CROUCH);
                    stateSwapped = true;
                    break;
                }
            case GhostEvent.EventType.UnCrouch:
                {
                    ghostCollider.size = standingColSize;
                    ghostCollider.center = Vector3.zero;
                    animationController.SwitchAnimSet(MoveMode.WALK);
                    stateSwapped = true;
                    break;
                }
            case GhostEvent.EventType.Sprint:
                {
                    ghostCollider.size = standingColSize;
                    ghostCollider.center = Vector3.zero;
                    animationController.SwitchAnimSet(MoveMode.SPRINT);
                    stateSwapped = true;
                    break;
                }
            case GhostEvent.EventType.UnSprint:
                {
                    ghostCollider.size = standingColSize;
                    ghostCollider.center = Vector3.zero;
                    animationController.SwitchAnimSet(MoveMode.WALK);
                    stateSwapped = true;
                    break;
                }
        }
    }

    private void TryInteract()
    {
        RaycastHit hit;

        //ghost interacts with a sphere cast to account for very slight positional errors.
        //due to the ghost movement being handled with lerps it may not be in the exact place the player was at the time of interaction
        //so a spherecast accounts for that by making the cast wider
        if(Physics.SphereCast(transform.position, interactRadius, recording[currentFrameIndex].cameraForward, out hit, interactDistance, interactableLayers))
        {            
            if (hit.collider.gameObject.TryGetComponent(out Interactable interactable))
            {
                interactable.Interact(gameObject);
            }
        }
    }
    
    public void ClearEventLog()
    {
        eventLog.Clear();
    }

    public void ToggleGhost(bool input)
    {
        if (input)
        {
            if (recording.Count > 2)
            {
                ghostState = GhostState.Inactive;
                currentFrameIndex = 0;
            }            

            transform.position = new Vector3(-100, -100, -100); //ensure the ghost is out of sight, as it cant be disabled and still allow for recording
            head.SetActive(!head.activeSelf);
            body.SetActive(!body.activeSelf);
            visualsActive = !visualsActive;
            if (!visualsActive)
            {
                GameUI.instance.UpdateGhostUIState(ghostUI.index, RecordState.Pause);
                GameUI.instance.UpdateGhostUITime(ghostUI.index, 0);
            }
            //need to wait a frame to wait for physics updates
            StartCoroutine(EnableColliderAfterFrame());

            if (!visualsActive && recording.Count > 0) recording.Clear();
        }
    }

    private void PerformGlitchEffect()
    {
        bodyRenderer.material.SetFloat("_GlitchAmount", restartGlitchValue);
        headRenderer.material.SetFloat("_GlitchAmount", restartGlitchValue);
        Invoke(nameof(ResetGlitch), 0.2f);
    }

    private void ResetGlitch()
    {
        bodyRenderer.material.SetFloat("_GlitchAmount", baseGlitchValue);
        headRenderer.material.SetFloat("_GlitchAmount", baseGlitchValue);
    }

    public int GetIndex()
    {
        return ghostUI.index;
    }

    private void ResetCanRecord()
    {
        canRecord = true;
    }

    private void SetAudioVolume(float volume)
    {
        glitchSound.volume = volume;
    }

    public void TogglePause()
    {
        if(ghostState == GhostState.Playing)
        {
            ghostState = GhostState.Paused;
        }
        else if(ghostState == GhostState.Paused && recording.Count >= 2)
        {
            ghostState = GhostState.Playing;
        }        
    }

    public void ToggleRewind()
    {

    }
}

public struct GhostFrame
{
    public Vector3 position;
    public Vector3 cameraForward;
    public Quaternion rotation;
    public Vector2 movementInput;
    public bool isCrouching;
    public bool isSprinting;
    public bool isJumping;
}

public struct GhostEvent
{
    public float time;
    public EventType type;

    //TODO: Add more events for crouching, sprinting etc.
    public enum EventType
    {
        Interact,     
        Crouch,
        UnCrouch,
        Sprint,
        UnSprint,
    }
}

public enum GhostState
{
    Recording,
    Playing,
    Inactive,
    Paused,
    Rewind
}