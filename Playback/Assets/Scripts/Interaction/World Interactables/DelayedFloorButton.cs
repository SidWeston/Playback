using System.Collections.Generic;
using UnityEngine;

public class DelayedFloorButton : FloorButton
{
    [SerializeField] private float releaseTime = 3f;
    private float r;

    private void Start()
    {
        if(powerline) powerline.material = lineOff;
        if(wallLight) wallLight.material = lightOff;
    }

    private void Update()
    {
        if (buttonDown)
        {
            if (t < pressTime)
            {
                r = releaseTime;
                t += Time.deltaTime;
                t = Mathf.Clamp(t, 0, pressTime);
            }
        }
        else
        {
            if (r > 0)
            {
                t = 0;
                r -= Time.deltaTime;
                r = Mathf.Clamp(r, 0, releaseTime);

                if(r <= 0)
                {
                    activatableObject.Deactivate();
                    active = false;
                    if(powerline) powerline.material = lineOff;
                    if(wallLight) wallLight.material = lightOff;
                }
            }
        }
        buttonTransform.localPosition = Vector3.Lerp(upPos, downPos, buttonDown ? t / pressTime : r / releaseTime);

    }

    public override void OnTriggerExit(Collider other)
    {
        if ((activatableLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            bool hasRemoved = false;
            if (overlappedObjects.Contains(other.gameObject))
            {
                overlappedObjects.Remove(other.gameObject);
                hasRemoved = true;
            }

            if (overlappedObjects.Count == 0 && hasRemoved)
            {
                if (buttonDown) buttonDown = false;
            }
        }
    }
}
