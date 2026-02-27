using System;
using System.Collections.Generic;
using UnityEngine;

public class ActivationGate : ActivatableObject
{
    //this is essentially an AND gate, as large as it needs to be based on how many activators (e.g buttons) are hooked up
    public List<Activator> activators = new List<Activator>();
    public List<ActivatableObject> activatableObjects = new List<ActivatableObject>();

    private bool powered;

    public override void Activate(GameObject activator)
    {
        for(int i = 0; i < activators.Count; i++)
        {
            if (!activators[i].powered)
            {
                //if it reaches here, one isn't powered so it fails
                return;
            }           
        }

        //if it reaches here, every connected source is powered, so activate the objects
        for(int i = 0; i < activatableObjects.Count; i++)
        {
            activatableObjects[i].Activate(activator);
        }
    }

    public override void Deactivate()
    {
        if (!powered) return;

        for (int i = 0; i < activatableObjects.Count; i++)
        {
            activatableObjects[i].Deactivate();
        }
    }
}