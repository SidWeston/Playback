using UnityEngine;

public class ObjectHolder : MonoBehaviour
{

    public Transform holdTransform;
    public PickupObject currentObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropObject()
    {
        currentObject.transform.parent = null;
    }

    public void PickupObject(PickupObject obj)
    {
        currentObject = obj;
        currentObject.transform.parent = holdTransform;
    }
}