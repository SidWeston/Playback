using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRewind : MonoBehaviour
{
    public PlayerMovement movement;
    public PlayerCamera playerCamera;
    public GhostController ghostController;
    public List<PlayerFrame> recording = new List<PlayerFrame>();

    public bool shouldRecord = true;
    public bool isRewinding = false;

    public float rewindTime = 5f;
    public float rewindDuration = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movement = GetComponent<PlayerMovement>();

        StartCoroutine(RecordFrames());

        InputManager.instance.rewindKey.keyPress += OnRewind;
    }

    IEnumerator RewindSmoothLerp()
    {
        isRewinding = true;
        shouldRecord = false;

        int frameCount = recording.Count;
        if (frameCount < 2)
        {
            CleanupAfterRewind();
            yield break; //nothing to rewind
        }

        float segmentTime = rewindDuration / (frameCount - 1);

        for (int i = frameCount - 1; i > 0; i--)
        {
            Vector3 startPos = recording[i].position;
            Quaternion startRot = recording[i].rotation;
            Vector3 endPos = recording[i - 1].position;
            Quaternion endRot = recording[i - 1].rotation;

            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / segmentTime;
                transform.position = Vector3.Lerp(startPos, endPos, t);
                transform.rotation = Quaternion.Slerp(startRot, endRot, t);
                yield return null; 
            }
        }

        CleanupAfterRewind(); //tidy up and restart recording
    }

    void CleanupAfterRewind()
    {
        recording.Clear(); //fresh timeline
        shouldRecord = true;
        isRewinding = false;

        movement.movementEnabled = true;
        playerCamera.camEnabled = true;

        StartCoroutine(RecordFrames());
    }

    public IEnumerator RecordFrames()
    {
        while(shouldRecord)
        {
            recording.Add(movement.RecordPlayerFrame());

            float cutoff = Time.time - rewindTime;
            while(recording.Count > 0 && recording[0].timeStamp < cutoff)
            {
                recording.RemoveAt(0);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void OnRewind(bool input)
    {
        if(input)
        {
            isRewinding = true;
            shouldRecord = false;

            movement.movementEnabled = false;
            playerCamera.camEnabled = false;

            if(ghostController.IsRecording())
            {
                ghostController.StopRecording();
            }

            StartCoroutine(RewindSmoothLerp());
        }
    }

}

public struct PlayerFrame
{
    public Vector3 position;
    public Quaternion rotation;
    public bool isCrouching;
    public float timeStamp;
}