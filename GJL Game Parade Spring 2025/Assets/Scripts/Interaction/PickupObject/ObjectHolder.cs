using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    public Transform playerCamera;
    public PickupObject currentObject;

    [SerializeField] private Transform holdPosition;

    [SerializeField] private float holdForce = 50f;
    [SerializeField] private float forceMultiplier = 100f;
    [SerializeField] private float maxHoldDistance = 2.0f;
    [Range(0, 1)]
    [SerializeField] private float dampingMulti = 0.95f;

    private float previousDrag;

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        if(currentObject != null)
        {
            Vector3 toHoldPoint = holdPosition.position - currentObject.transform.position;
            float scaledForce = holdForce + (toHoldPoint.magnitude * forceMultiplier);

            currentObject.rb.AddForce(toHoldPoint * scaledForce);
            //smooth the force over time, for stability
            float scaledDamp = toHoldPoint.magnitude * dampingMulti;
            currentObject.rb.angularVelocity *= scaledDamp;

            if(toHoldPoint.magnitude > maxHoldDistance)
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
        //back down so it falls properly
        currentObject.rb.linearDamping = previousDrag;
        currentObject.transform.parent = null;
        currentObject = null;

        InputManager.instance.interactKey.keyPress -= OnDrop;
    }

    public void PickupObject(PickupObject obj)
    {
        currentObject = obj;
        //helps with stability
        previousDrag = currentObject.rb.linearDamping;
        currentObject.rb.linearDamping = 5f;

        InputManager.instance.interactKey.keyPress += OnDrop;
    }

    private void OnDrop(bool input)
    {
        if(input && currentObject)
        {
            DropObject();
        }
    }
}