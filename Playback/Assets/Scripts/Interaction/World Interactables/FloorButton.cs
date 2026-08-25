using System.Collections.Generic;
using UnityEngine;

public class FloorButton : ButtonBase
{    
    [SerializeField] protected LayerMask activatableLayers;

    [SerializeField] protected List<GameObject> overlappedObjects = new List<GameObject>();
    private HashSet<GameObject> touchedThisStep = new HashSet<GameObject>();
    
    [SerializeField] protected Transform buttonTransform;
    [SerializeField] protected Vector3 upPos, downPos;

    protected bool buttonDown = false;
    protected float pressTime = 0.25f;
    protected float t;
  
    private void Start()
    {
        if(powerline)  powerline.material = lineOff; 
        if(wallLight)  wallLight.material = lightOff;
    }

    private void Update()
    {
        if(buttonDown)
        {
            if (t < pressTime)
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
        t = Mathf.Clamp(t, 0, pressTime);
        buttonTransform.localPosition = Vector3.Lerp(upPos, downPos, t / pressTime);
    }

    private void FixedUpdate()
    {
        //this catches any colliders that are disabled rather than leaving naturally, 
        //otherwise they get stuck in the overlapped objects list and the button will be stuck down
        if (overlappedObjects.Count > 0)
        {
            bool removedAny = overlappedObjects.RemoveAll(obj => obj == null || !touchedThisStep.Contains(obj)) > 0;

            if (removedAny && overlappedObjects.Count == 0)
            {
                Deactivate();
            }
        }

        touchedThisStep.Clear();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if((activatableLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            if (!buttonDown) buttonDown = true;
            powered = true;
            if (!overlappedObjects.Contains(other.gameObject))
                overlappedObjects.Add(other.gameObject);
            ActivateObjects(gameObject);            
            if(powerline)   powerline.material = lineOn;
            if(wallLight)   wallLight.material = lightOn;
        }
    }

    public virtual void OnTriggerStay(Collider other)
    {
        if ((activatableLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            touchedThisStep.Add(other.gameObject);
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if ((activatableLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            bool hasRemoved = overlappedObjects.Remove(other.gameObject);

            if(overlappedObjects.Count == 0 && hasRemoved)
            {
                Deactivate();
            }
        }
    }

    private void Deactivate()
    {
        DeactivateObjects();
        if (buttonDown) buttonDown = false;
        powered = false;
        if(powerline)  powerline.material = lineOff;
        if(wallLight)   wallLight.material = lightOff;
    }
}