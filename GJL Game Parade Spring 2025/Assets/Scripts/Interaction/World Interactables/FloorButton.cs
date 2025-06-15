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
            }
        }
    }
}
