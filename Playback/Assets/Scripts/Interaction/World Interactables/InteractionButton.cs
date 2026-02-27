using Unity.VisualScripting;
using UnityEngine;

public class InteractionButton : ButtonBase, Interactable
{

    //if the button is pressed once and activates, or can be pressed again to deactivate
    [SerializeField] private bool toggleable = false;    

    [SerializeField] private bool timed = false;
    [SerializeField] private float timer = 2.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (powerline) powerline.material = lineOff;
        if (wallLight) wallLight.material = lightOff;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(GameObject interactor)
    {
        if (!toggleable)
        {
            if (!powered && !IsInvoking())
            {
                ActivateObjects(interactor);
                powered = true;
                if (powerline) powerline.material = lineOn;
                if (wallLight) wallLight.material = lightOn;
            }

        }
        else if (toggleable)
        {
            if (!powered)
            {
                ActivateObjects(interactor);
                powered = true;
                if (powerline) powerline.material = lineOn;
                if (wallLight) wallLight.material = lightOn;
            }
            else
            {
                DeactivateObjects();
                powered = false;
                if (powerline) powerline.material = lineOff;
                if (wallLight) wallLight.material = lightOff;
            }

        }

        if(timed && powered)
        {
            Invoke(nameof(DeactivateOnTimer), timer);
        }
        else if(timed && !powered)
        {
            CancelInvoke(nameof(DeactivateOnTimer));
        }
    }

    private void DeactivateOnTimer()
    {
        DeactivateObjects();
        powered = false;
        if (powerline) powerline.material = lineOff;
        if (wallLight) wallLight.material = lightOff;
    }
}