using System.Collections.Generic;
using UnityEngine;

public class MultiReceiver : ActivatableObject
{
    public List<EnergyReceiver> receivers = new List<EnergyReceiver>();

    public ActivatableObject activatable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Activate(GameObject activator)
    {
        foreach(EnergyReceiver receiver in receivers)
        {
            if(!receiver.powered)
            {
                return;
            }
        }
        activatable.Activate(gameObject);
    }

    public override void Deactivate()
    {

    }
}