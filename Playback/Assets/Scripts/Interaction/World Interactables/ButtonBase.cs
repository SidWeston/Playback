using System.Collections.Generic;
using UnityEngine;

public class ButtonBase : MonoBehaviour
{    
    [SerializeField] protected List<ActivatableObject> activatableObjects;

    public bool active = false;

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

    protected void ActivateObjects(GameObject activator)
    {
        for(int i = 0; i < activatableObjects.Count; i++)
        {
            activatableObjects[i].Activate(activator);
        }
    }

    protected void DeactivateObjects()
    {
        for(int i = 0; i < activatableObjects.Count; i++)
        {
            activatableObjects[i].Deactivate();
        }
    }
}