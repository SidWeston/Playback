using UnityEngine;

public class EnergyReceiver : MonoBehaviour
{
    public ActivatableObject poweredObj;

    public bool powered = false;

    //line renderers
    [SerializeField] private LineRenderer powerline;
    [SerializeField] private Material lineOff, lineOn;
    //powerlight
    [SerializeField] private Renderer wallLight;
    [SerializeField] private Material lightOff, lightOn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        powerline.material = lineOff; //start off
        wallLight.material = lightOff;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PowerOn()
    {
        if(!powered)
        {
            powered = true;
            poweredObj.Activate(gameObject);
            powerline.material = lineOn;
            wallLight.material = lightOn;
        }
    }
}