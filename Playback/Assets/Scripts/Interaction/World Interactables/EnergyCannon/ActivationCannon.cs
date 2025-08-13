using UnityEngine;

public class ActivationCannon : EnergyCannon
{
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
        GameObject ball = Instantiate(energyBall, spawnLocation.position, spawnLocation.rotation);
        ball.TryGetComponent(out EnergyBall energy);
        energy.SetOwner(this);
    }

    public override void Deactivate()
    {

    }
}