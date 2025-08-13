using UnityEngine;

public class LoopButton : FloorButton
{
    private float loopTimer = 3f;

    private void Start()
    {
        if (powerline) powerline.material = lineOff;
        if (wallLight) wallLight.material = lightOff;
    }

    private void Update()
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
            InvokeRepeating("Activate", 0, loopTimer);
            if (powerline) powerline.material = lineOn;
            if (wallLight) wallLight.material = lightOn;
        }
    }

    private void Activate()
    {
        activatableObject.Activate(gameObject);
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
                activatableObject.Deactivate();
                CancelInvoke("Activate");
                if (buttonDown) buttonDown = false;
                active = false;
                if (powerline) powerline.material = lineOff;
                if (wallLight) wallLight.material = lightOff;
            }
        }
    }
}
