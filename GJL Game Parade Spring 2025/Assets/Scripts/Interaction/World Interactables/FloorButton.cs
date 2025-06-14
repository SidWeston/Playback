using Animancer;
using System.Collections.Generic;
using UnityEngine;

public class FloorButton : MonoBehaviour
{
    [SerializeField] private ActivatableObject activatableObject;

    [SerializeField] private LayerMask activatableLayers;

    [SerializeField] private List<GameObject> overlappedObjects = new List<GameObject>();

    private bool buttonDown = false;

    [SerializeField] private Transform buttonTransform;
    [SerializeField] private Vector3 upPos, downPos;
    private float pressTime = 0.25f;
    private float t;

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

    public void OnTriggerEnter(Collider other)
    {
        if((activatableLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            overlappedObjects.Add(other.gameObject);
            activatableObject.Activate();
            if (!buttonDown) buttonDown = true;
        }
    }

    public void OnTriggerExit(Collider other)
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
            }
        }
    }
}
