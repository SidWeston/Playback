using Unity.VisualScripting;
using UnityEngine;

public class InteractionButton : MonoBehaviour, Interactable
{

    [SerializeField] protected ActivatableObject activatableObject;

    //if the button is pressed once and activates, or can be pressed again to deactivate
    [SerializeField] private bool toggleable = false;
    private bool pressed = false;

    [SerializeField] protected LineRenderer powerline;
    [SerializeField] protected Material lineOff, lineOn;
    //powerlight
    [SerializeField] protected Renderer wallLight;
    [SerializeField] protected Material lightOff, lightOn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(GameObject interactor)
    {
        Debug.Log("interacted");
        if(!toggleable)
        {
            if(!pressed)
            {
                activatableObject.Activate(interactor);
                pressed = true;
            }

        }
        else if(toggleable)
        {
            if(!pressed)
            {
                activatableObject.Activate(interactor);
                pressed = true;
            }
            else
            {
                activatableObject.Deactivate();
                pressed = false;
            }

        }
    }
}