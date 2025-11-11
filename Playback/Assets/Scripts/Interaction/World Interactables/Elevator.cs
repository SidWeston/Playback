using UnityEngine;

public class Elevator : ActivatableObject
{
    private Vector3 startPos;
    [SerializeField] private Vector3 endPos;

    [SerializeField] private float elevatorSpeed = 3.0f;
    private float elevatorTime; //how long it will take the elevator to reach the top based on the speed and distance
    private float time;

    private bool active = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //T = S * D
        elevatorTime = elevatorSpeed * Vector3.Distance(startPos, endPos);
    }

    // Update is called once per frame
    void Update()
    {
        if(active && time < elevatorTime)
        {        
            time += Time.deltaTime;
            float t = time / elevatorTime;
            transform.position = Vector3.Lerp(startPos, endPos, t);
        }
        else if(!active && time > 0)
        {
            time -= Time.deltaTime;
            float t = time / elevatorTime;
            transform.position = Vector3.Lerp(startPos, endPos, t);
        }
    }

    public override void Activate(GameObject activator)
    {
        active = true;
    }

    public override void Deactivate()
    {
        active = false;
    }   

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(endPos, 0.25f);
    }
}
