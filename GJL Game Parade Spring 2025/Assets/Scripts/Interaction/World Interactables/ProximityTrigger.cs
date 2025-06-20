using UnityEngine;

public class ProximityTrigger : MonoBehaviour
{
    public ActivatableObject obj;

    public bool deactivateWhenLeft = false;

    [SerializeField] private LayerMask activatableLayers;

    //powerlight
    [SerializeField] private Renderer wallLight;
    [SerializeField] private Material lightOff, lightOn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wallLight.material = lightOff;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((activatableLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            obj.Activate(gameObject);
            wallLight.material = lightOn;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!deactivateWhenLeft) return;

        if ((activatableLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            obj.Deactivate();
            wallLight.material = lightOff;
        }
    }
}