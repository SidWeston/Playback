using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlayer : MonoBehaviour, Interactable
{
    public PlayerMovement target;

    public List<GhostFrame> recording = new List<GhostFrame>();

    private bool shouldRecord = true;

    private float travelTimer = 0f;
    private Vector3 previousPosition;

    [SerializeField] private float ghostDelay = 10.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        previousPosition = transform.position;
        StartCoroutine(RecordFrame());
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

        if (!found)
        {
            Debug.Log("not found");
            return;
        }

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
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Interact()
    {
        //reset the ghost, if the player gets stuck
    }
}

public struct GhostFrame
{
    public Vector3 position;
    public Quaternion rotation;
    public bool isCrouching;
    public float timeStamp;
}