using Animancer;
using System.Collections.Generic;
using UnityEngine;

public class FloorButton : MonoBehaviour
{
    [SerializeField] protected ActivatableObject activatableObject;

    [SerializeField] protected LayerMask activatableLayers;

    [SerializeField] protected List<GameObject> overlappedObjects = new List<GameObject>();

    protected bool buttonDown = false;
    public bool active = false;

    [SerializeField] protected Transform buttonTransform;
    [SerializeField] protected Vector3 upPos, downPos;
    protected float pressTime = 0.25f;
    protected float t;

    //line renderers
    [SerializeField] protected LineRenderer powerline;
    [SerializeField] protected Material lineOff, lineOn;
    //powerlight
    [SerializeField] protected Renderer wallLight;
    [SerializeField] protected Material lightOff, lightOn;

    private void Start()
    {
        if(powerline)   powerline.material = lineOff; 
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

    public virtual void OnTriggerEnter(Collider other)
    {
        if((activatableLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            if (!buttonDown) buttonDown = true;
            active = true;
            overlappedObjects.Add(other.gameObject);
            activatableObject.Activate(gameObject);
            if(powerline)   powerline.material = lineOn;
            if(wallLight)   wallLight.material = lightOn;
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if ((activatableLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            bool hasRemoved = false;
            if (overlappedObjects.Contains(other.gameObject))
            {
                overlappedObjects.Remove(other.gameObject);
                hasRemoved = true;
            }

            if(overlappedObjects.Count == 0 && hasRemoved)
            {
                activatableObject.Deactivate();
                if (buttonDown) buttonDown = false;
                active = false;
                if(powerline)  powerline.material = lineOff;
                if(wallLight)   wallLight.material = lightOff;
            }
        }
    }
}
