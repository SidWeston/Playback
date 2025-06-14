using UnityEngine;

public class Door : ActivatableObject
{
    public Vector3 closedPos;
    public Vector3 openPos;

    public float openTime = 1f;
    private float t;

    private bool open = false; //door starts closed

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(open)
        {
            if(t < openTime)
            {
                t += Time.deltaTime;
            }
        }
        else
        {
            if(t > 0)
            {
                t -= Time.deltaTime;
            }
        }
        t = Mathf.Clamp(t, 0, openTime);
        transform.position = Vector3.Lerp(closedPos, openPos, t / openTime);
    }

    public override void Activate()
    {
        if(!open)
        {
            open = true;
        }
    }

    public override void Deactivate()
    {
        if(open)
        {
            open = false;
        }
    }
}
