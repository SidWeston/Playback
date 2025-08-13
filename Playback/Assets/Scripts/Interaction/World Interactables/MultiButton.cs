using System.Collections.Generic;
using UnityEngine;

public class MultiButton : FloorButton
{
    public List<ActivatableObject> objects = new List<ActivatableObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (powerline) powerline.material = lineOff;
        if (wallLight) wallLight.material = lightOff;
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonDown)
        {
            if (t < pressTime)
            {
                t += Time.deltaTime;
            }
        }
        else
        {
            if (t > 0)
            {
                t -= Time.deltaTime;
            }
        }
        t = Mathf.Clamp(t, 0, pressTime);
        buttonTransform.localPosition = Vector3.Lerp(upPos, downPos, t / pressTime);
    }

    public override void OnTriggerEnter(Collider other)
    {
        if ((activatableLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            if (!buttonDown) buttonDown = true;
            active = true;
            overlappedObjects.Add(other.gameObject);
            foreach(ActivatableObject obj in objects)
            {
                obj.Activate(gameObject);
            }
            if (powerline) powerline.material = lineOn;
            if (wallLight) wallLight.material = lightOn;
        }
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
                foreach(ActivatableObject obj in objects)
                {
                    obj.Deactivate();
                }
                if (buttonDown) buttonDown = false;
                active = false;
                if (powerline) powerline.material = lineOff;
                if (wallLight) wallLight.material = lightOff;
            }
        }
    }
}
