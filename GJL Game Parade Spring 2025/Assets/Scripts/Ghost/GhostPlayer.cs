using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using UnityEngine;

public class GhostPlayer : MonoBehaviour
{
    [SerializeField] private PlayerMovement target;

    private List<GhostFrame> recording = new List<GhostFrame>();

    private bool isRecording = false;

    [SerializeField] private float frameInterval = 0.1f;
    [SerializeField] private float recordDuration = 5f;

    [SerializeField] private GhostAnimationController animationController;
    [SerializeField] private BoxCollider ghostCollider;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private GameObject head, body;
    [SerializeField] private Material ghostMat;

    private int currentFrameIndex = 0;
    private float frameTimer = 0f;
    private bool isPlaying = false;
    private bool active = false;

    private bool overlappingPlayer = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.instance.ghostKey.keyPress += ToggleGhost;
        InputManager.instance.recordKey.keyPress += StartRecording;
        ResetGlitch(); //ensure glitch effect isnt set to high by default
        active = false; //assume ghost starts turned off
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying || recording.Count < 2) return;

        frameTimer += Time.deltaTime;

        while(frameTimer > frameInterval)
        {
            frameTimer -= frameInterval;
            currentFrameIndex++;
            if(currentFrameIndex >= recording.Count - 1)
            {
                currentFrameIndex = 0;
                ghostMat.SetFloat("_GlitchAmount", 5);
                Invoke("ResetGlitch", 0.2f);
                if (CheckForPlayerOverlap())
                {
                    overlappingPlayer = true;
                    Physics.IgnoreCollision(ghostCollider, target.gameObject.GetComponent<CharacterController>(), true);
                }
            }
        }

        if (overlappingPlayer && !!CheckForPlayerOverlap())
        {
            overlappingPlayer = false;
            Physics.IgnoreCollision(ghostCollider, target.gameObject.GetComponent<CharacterController>(), false);
        }

        GhostFrame a = recording[currentFrameIndex];
        GhostFrame b = recording[currentFrameIndex + 1];
        float t = frameTimer / frameInterval;

        Vector3 position = Vector3.Lerp(a.position, b.position, t);
        Quaternion rotation = Quaternion.Slerp(a.rotation, b.rotation, t);

        transform.position = position;
        transform.rotation = rotation;

        animationController.PlayMovementAnimation(a.movementInput);
        animationController.SwitchAnimSet(a.isCrouching, a.isSprinting);

        ghostCollider.size = new Vector3(ghostCollider.size.x, a.isCrouching ? 1 : 2, ghostCollider.size.z);
        ghostCollider.center = a.isCrouching ? new Vector3(0, -0.25f, 0) : Vector3.zero;
    }

    private void ResetGlitch()
    {
        ghostMat.SetFloat("_GlitchAmount", 0.1f);
    }

    public void StopRecording()
    {
        StopCoroutine(RecordFrame());
    }

    private void UpdateOverlapState()
    {
        bool shouldIgnore = CheckForPlayerOverlap();

        if (shouldIgnore && !overlappingPlayer)
        {
            overlappingPlayer = true;
            Physics.IgnoreCollision(ghostCollider, target.GetComponent<CharacterController>(), true);
        }
        else if (!shouldIgnore && overlappingPlayer)
        {
            overlappingPlayer = false;
            Physics.IgnoreCollision(ghostCollider, target.GetComponent<CharacterController>(), false);
        }
    }

    public bool CheckForPlayerOverlap()
    {
        Collider[] hits = Physics.OverlapBox(ghostCollider.bounds.center, ghostCollider.bounds.extents,
            ghostCollider.transform.rotation, playerLayer);
        if(hits.Length > 0)
        {
            return false;
        }
        return true;
    }

    public IEnumerator RecordFrame()
    {
        isRecording = true;
        if(active) ToggleGhost(true);
        List<GhostFrame> newFrames = new List<GhostFrame>();
        float timer = 0f;

        while (timer < recordDuration)
        {
            newFrames.Add(target.RecordFrame());
            yield return new WaitForSeconds(frameInterval);
            timer += frameInterval;
        }

        if (!active) ToggleGhost(true);
        recording = newFrames;
        currentFrameIndex = 0;
        ghostMat.SetFloat("_GlitchAmount", 5);
        Invoke("ResetGlitch", 0.2f);
        if (CheckForPlayerOverlap())
        {
            overlappingPlayer = true;
            Physics.IgnoreCollision(ghostCollider, target.gameObject.GetComponent<CharacterController>(), true);
        }
        frameTimer = 0;
        transform.position = recording[0].position;
        transform.rotation = recording[0].rotation;
        isPlaying = true;
        isRecording = false;
    }

    private void ToggleGhost(bool input)
    {
        if (input)
        {
            if (recording.Count > 2)
            {
                isPlaying = !isPlaying;
                currentFrameIndex = 0;
            }
            else isPlaying = false;
            transform.position = Vector3.zero;
            head.SetActive(!head.activeSelf);
            body.SetActive(!body.activeSelf);
            active = !active;
            //need to wait a frame to wait for physics updates
            StartCoroutine(DisableGhostAfterFrame());
        }
    }

    private IEnumerator DisableGhostAfterFrame()
    {
        yield return new WaitForFixedUpdate();
        ghostCollider.enabled = !ghostCollider.enabled;
    }

    private void StartRecording(bool input)
    {
        if(input && !isRecording)
        {
            StartCoroutine(RecordFrame());
        }
    }
}

public struct GhostFrame
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector2 movementInput;
    public bool isCrouching;
    public bool isSprinting;
    public bool isJumping;
    public float timeStamp;
}