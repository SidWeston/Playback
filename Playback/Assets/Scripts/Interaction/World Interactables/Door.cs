using UnityEngine;

public class Door : ActivatableObject
{
    [SerializeField] private Vector3 doorOpenOffset;
    [SerializeField] private Vector3 doorRotOffset;
    private Vector3 reverseDoorRot;

    private Vector3 closedPos;
    private Vector3 openPos;
    private Quaternion closedRot;
    private Quaternion openRot;

    //only if the door mesh isnt on this object, i.e its on a child 
    [SerializeField] private GameObject doorObj; 

    public float openTime = 1f;
    protected float t;

    protected bool open = false;    

    [SerializeField] protected AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!doorObj) doorObj = gameObject;        
        reverseDoorRot = doorRotOffset * -1;

        //setup positions and offsets
        closedPos = transform.localPosition;
        openPos = closedPos + doorOpenOffset;

        closedRot = transform.localRotation;
        openRot = closedRot * Quaternion.Euler(doorRotOffset);

        if(TryGetComponent(out AudioSource source))
        {
            audioSource = source;
            Settings.instance.effectsVolumeChange += SetAudioVolume;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(open)
        {
            if(t < openTime)
            {
                t += Time.deltaTime;
            }
        }
        else
        {
            if(t > 0)
            {
                t -= Time.deltaTime;
            }
        }
        t = Mathf.Clamp(t, 0, openTime);
        doorObj.transform.localPosition = Vector3.Lerp(closedPos, openPos, t / openTime);
        doorObj.transform.localRotation = Quaternion.Lerp(closedRot, openRot, t / openTime);
    }

    public override void Activate(GameObject activator)
    {
        if(!open)
        {
            open = true;            
            if(audioSource) audioSource.Play();
        }
    }

    public override void Deactivate()
    {
        if(open)
        {
            open = false;
            if(audioSource) audioSource.Play();
        }
    }

    private void SetAudioVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void SetOpenDirection(int direction)
    {
        if(direction == 1)
        {
            openRot = closedRot * Quaternion.Euler(doorRotOffset);
        }
        else if(direction == -1)
        {
            openRot = closedRot * Quaternion.Euler(reverseDoorRot);
        }        
    }
}
