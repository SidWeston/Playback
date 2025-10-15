using Unity.VisualScripting;
using UnityEngine;

public class InteractionButton : ButtonBase, Interactable
{

    [SerializeField] protected ActivatableObject activatableObject;

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
            if (!active)
            {
                activatableObject.Activate(interactor);
                active = true;
                if (powerline) powerline.material = lineOn;
                if (wallLight) wallLight.material = lightOn;
            }

        }
        else if (toggleable)
        {
            if (!active)
            {
                activatableObject.Activate(interactor);
                active = true;
                if (powerline) powerline.material = lineOn;
                if (wallLight) wallLight.material = lightOn;
            }
            else
            {
                activatableObject.Deactivate();
                active = false;
                if (powerline) powerline.material = lineOff;
                if (wallLight) wallLight.material = lightOff;
            }

        }

        if(timed && active)
        {
            Invoke(nameof(DeactivateOnTimer), timer);
        }
        else if(timed && !active)
        {
            CancelInvoke(nameof(DeactivateOnTimer));
        }
    }

    private void DeactivateOnTimer()
    {
        activatableObject.Deactivate();
        active = false;
        if (powerline) powerline.material = lineOff;
        if (wallLight) wallLight.material = lightOff;
    }
}