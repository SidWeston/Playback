using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//door with multiple activations required
public class MultiDoor : ActivatableObject
{
    public List<FloorButton> buttons = new List<FloorButton>();

    public Vector3 closedPos;
    public Vector3 openPos;

    public float openTime = 1f;
    private float t;

    private bool open = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        closedPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (open)
        {
            if (t < openTime)
            {
                t += Time.deltaTime;
            }
        }
        else
        {
            if (t > 0)
            {
                t -= Time.deltaTime;
            }
        }
        t = Mathf.Clamp(t, 0, openTime);
        transform.position = Vector3.Lerp(closedPos, openPos, t / openTime);
    }

    public override void Activate(GameObject activator)
    {
        foreach (FloorButton button in buttons)
        {
            if (!button.active)
            {
                return;
            }
        }
        open = true;
    }

    public override void Deactivate()
    {
        if(open)
        {
            open = false;
        }
    }
}