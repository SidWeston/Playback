using System.Collections.Generic;
using UnityEngine;

public class ButtonSequencer : ActivatableObject
{
    public List<FloorButton> buttons = new List<FloorButton>();
    public ActivatableObject activatable;

    public List<LightIndicator> indicators = new List<LightIndicator>();

    public int current = 0;

    public bool complete = false;
    private bool started = false;
    public float resetTimer = 5f;
    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(started && !complete)
        {
            timer += Time.deltaTime;
            if (timer >= resetTimer)
            {
                started = false;
                current = 0;
                timer = 0;
                foreach (LightIndicator indicator in indicators)
                {
                    indicator.Deactivate();
                }
            }
        }
    }

    public override void Activate(GameObject activator)
    {
        if (complete) return;
        if(activator.TryGetComponent(out FloorButton button))
        {
            if (buttons[current] == button)
            {
                if (!started) started = true;
                indicators[current].Activate(gameObject);
                current++;
                timer = 0;
                if (current >= buttons.Count)
                {
                    CompletePuzzle();   
                }
            }
            else
            {
                started = false;
                current = 0;
                foreach(LightIndicator indicator in indicators)
                {
                    indicator.Deactivate();
                }
            }
        }
    }

    public override void Deactivate()
    {
        
    }
    
    public void CompletePuzzle()
    {
        complete = true;
        activatable.Activate(gameObject);
    }
}