using UnityEngine;
using System.Collections.Generic;
using System;

public class DoubleDoor : ActivatableObject
{
    [SerializeField] private List<Door> doors = new List<Door>();

    [SerializeField] private List<Renderer> wallLights = new List<Renderer>();
    [SerializeField] protected Material lightOff, lightOn;

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
        Vector3 directionToPlayerFromDoor = activator.transform.position - transform.position;        
        int direction = Vector3.Dot(directionToPlayerFromDoor, transform.forward) > 0.5f ? 1 : -1;
        for (int i = 0; i < doors.Count; i++)
        {            
            doors[i].SetOpenDirection(direction);
            doors[i].Activate(activator);
        }

        for(int i = 0; i < wallLights.Count; i++)
        {
            wallLights[i].material = lightOn;
        }
    }

    public override void Deactivate()
    {
        for (int i = 0; i < doors.Count; i++)
        {
            doors[i].Deactivate();
        }

        for (int i = 0; i < wallLights.Count; i++)
        {
            wallLights[i].material = lightOff;
        }
    }
}