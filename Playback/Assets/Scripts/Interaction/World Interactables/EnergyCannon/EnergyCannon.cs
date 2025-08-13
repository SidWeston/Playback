using UnityEngine;

public class EnergyCannon : ActivatableObject
{
    public GameObject energyBall;

    public Transform spawnLocation;

    [SerializeField] private float spawnInterval = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("FireEnergyBall", 0, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireEnergyBall()
    {
        GameObject ball = Instantiate(energyBall, spawnLocation.position, spawnLocation.rotation);
        ball.TryGetComponent(out EnergyBall energy);
        energy.SetOwner(this);
    }

    public override void Activate(GameObject activator)
    {
        
    }

    public override void Deactivate()
    {
        
    }
}