using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostPlayer : MonoBehaviour
{
    [SerializeField] private PlayerMovement target;

    private List<GhostFrame> recording = new List<GhostFrame>();

    private bool shouldRecord = true;

    [SerializeField] private float ghostDelay = 10.0f;

    [SerializeField] private GhostAnimationController animationController;
    [SerializeField] private BoxCollider ghostCollider;
    [SerializeField] private LayerMask playerLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(RecordFrame());

        InputManager.instance.ghostKey.keyPress += ToggleCollision;
    }

    // Update is called once per frame
    void Update()
    {
        float ghostTime = Time.time - ghostDelay;

        if (recording == null || recording.Count < 2) return;

        //find the two frames around the current ghost time
        GhostFrame a = default;
        GhostFrame b = default;
        bool found = false;

        for(int i = 0; i < recording.Count - 1; i++)
        {
            if (recording[i].timeStamp <= ghostTime && recording[i + 1].timeStamp >= ghostTime)
            {
                a = recording[i];
                b = recording[i + 1];
                found = true;
                break;
            }
        }

        if (!found) return;

        //play animations
        animationController.PlayMovementAnimation(a.movementInput);

        animationController.SwitchAnimSet(a.isCrouching, a.isSprinting);

        if(a.isCrouching)
        {
            ghostCollider.size = new Vector3(ghostCollider.size.x, 1, ghostCollider.size.z);
            ghostCollider.center = new Vector3(0, -0.25f, 0);
        }
        else
        {
            ghostCollider.size = new Vector3(ghostCollider.size.x, 2, ghostCollider.size.z);
            ghostCollider.center = Vector3.zero;
        }

        //check if player is stood on top of the ghost, and if so clamp y position to stop the ghost flying up and pushing the player
        if(Physics.CheckSphere(transform.position + Vector3.up, 0.5f, playerLayer))
        {
            a.position.y = transform.position.y;
            b.position.y = transform.position.y;
        }

        //move to next point smoothly
        float t = Mathf.InverseLerp(a.timeStamp, b.timeStamp, ghostTime);
        transform.position = Vector3.Lerp(a.position, b.position, t);
        transform.rotation = Quaternion.Slerp(a.rotation, b.rotation, t);
    }

    public void StartRecording()
    {

    }

    public IEnumerator RecordFrame()
    {
        while(shouldRecord)
        {
            recording.Add(target.RecordFrame());

            float minTime = Time.time - ghostDelay - 1f;
            while(recording.Count > 0 && recording[0].timeStamp < minTime)
            { 
                recording.RemoveAt(0);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void ToggleCollision(bool input)
    {
        if (input)
        {
            if(ghostCollider.enabled)
            {
                ghostCollider.enabled = false;
            }
            else
            {
                ghostCollider.enabled = true;
            }
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