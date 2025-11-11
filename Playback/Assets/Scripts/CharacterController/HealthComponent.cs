using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float health = 100.0f;

    private bool regenerating;
    [SerializeField] private float regenDelay = 1.5f;
    [SerializeField] private float regenRate = 15.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(regenerating)
        {
            health += regenRate * Time.deltaTime;
        }
    }

    public void TakeDamage(float damage)
    {
        if (regenerating) regenerating = false;
        health -= damage;
        if(health <= 0)
        {
            KillPlayer();
        }
        else
        {
            Invoke(nameof(StartRegeneration), regenDelay);
        }
    }

    public void KillPlayer()
    {
        //stop all inputs and movements
        //then restart the level/checkpoint
    }

    private void StartRegeneration()
    {
        regenerating = true;
    }
}