using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    public Transform playerCamera;
    public PickupObject currentObject;


    [SerializeField] private Transform holdPosition;
    [SerializeField] private float dropThreshold = 1f;

    private void Update()
    {
        if(currentObject != null)
        {
            if(Vector3.Distance(holdPosition.position, currentObject.transform.position) > dropThreshold)
            {
                DropObject();
            }
        }
    }

    public void DropObject()
    {
        if (currentObject.gameObject.TryGetComponent(out Rigidbody rb))
        {
            rb.useGravity = true;
        }
        currentObject.transform.parent = null;
        currentObject = null;
    }

    public void PickupObject(PickupObject obj)
    {
        currentObject = obj;
        if (currentObject.gameObject.TryGetComponent(out Rigidbody rb))
        {
            rb.useGravity = false;
        }
        currentObject.transform.parent = playerCamera;
        holdPosition.localPosition = currentObject.transform.localPosition;
    }
}