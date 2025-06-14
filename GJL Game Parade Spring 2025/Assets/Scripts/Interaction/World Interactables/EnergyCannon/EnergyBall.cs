using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    public EnergyCannon owner;
    [SerializeField] private Collider trigger;
    [SerializeField] private LayerMask refireLayers; //player and ghost
    [SerializeField] private LayerMask activateLayers; //receivers etc.

    [SerializeField] private float velocity;
    public int redirects = 0;
    [SerializeField] private int maxRedirects = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (redirects > maxRedirects)
        {
            Destroy(gameObject);
        }
        Invoke("EnableTrigger", 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * velocity * Time.deltaTime;
    }

    public void EnableTrigger()
    {
        trigger.enabled = true;
    }

    public void SetOwner(EnergyCannon newOwner)
    {
        owner = newOwner;
    }

    public void SetTrajectory(Quaternion newRot)
    {
        transform.rotation = newRot;
    }

    public void OnTriggerEnter(Collider other)
    {
        if ((refireLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            //refire
            other.gameObject.TryGetComponent(out BallRedirector redirector);
            redirector.SetPrefab(owner.energyBall);
            redirector.InstantiatePrefab(owner, redirects);
            Destroy(gameObject);
        }
        else if((activateLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            other.gameObject.TryGetComponent(out EnergyReceiver obj);
            obj.PowerOn();
            Destroy(gameObject);
        }
    }
}