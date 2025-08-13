using System.Collections.Generic;
using UnityEngine;

public class EnergyWallSwitcher : ActivatableObject
{
    //have to manually set what walls should switch. if they start off, they will switch to on, and on to off. have a mix to have them alternate
    public List<EnergyWall> walls = new List<EnergyWall>();

    [SerializeField] private float switchTimer = 10f;
    private float timer;

    public bool active = true;

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            timer += Time.deltaTime;
            if(timer >= switchTimer)
            {
                SwapWalls();
                timer = 0;
            }
        }
    }

    private void SwapWalls()
    {
        for(int i = 0; i < walls.Count; i++)
        {
            walls[i].gameObject.SetActive(!walls[i].gameObject.activeSelf);
        }
    }

    public override void Activate(GameObject activator)
    {
        active = !active;
    }

    public override void Deactivate()
    {
        
    }
}