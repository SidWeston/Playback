using UnityEngine;

public class EnergyReceiver : MonoBehaviour
{
    public ActivatableObject poweredObj;

    public bool powered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PowerOn()
    {
        if(!powered)
        {
            poweredObj.Activate(gameObject);
            powered = true;
        }
    }
}