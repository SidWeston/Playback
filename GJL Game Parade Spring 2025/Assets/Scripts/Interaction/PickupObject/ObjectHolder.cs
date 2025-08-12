using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    public Transform playerCamera;
    public PickupObject largeObject;
    public SmallPickupObject smallObject;

    [SerializeField] private Transform largeHoldPos;
    [SerializeField] private Transform smallHoldPos;

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
        if(largeObject != null)
        {
            Vector3 toHoldPoint = largeHoldPos.position - largeObject.transform.position;
            float scaledForce = holdForce + (toHoldPoint.magnitude * forceMultiplier);

            largeObject.rb.AddForce(toHoldPoint * scaledForce);
            //smooth the force over time, for stability
            float scaledDamp = toHoldPoint.magnitude * dampingMulti;
            largeObject.rb.angularVelocity *= scaledDamp;

            if(toHoldPoint.magnitude > maxHoldDistance)
            {
                DropObject();
            }
        }
    }

    public void DropObject()
    {
        if (largeObject.gameObject.TryGetComponent(out Rigidbody rb))
        {
            rb.useGravity = true;
        }
        //back down so it falls properly
        largeObject.rb.linearDamping = previousDrag;
        largeObject.transform.parent = null;
        largeObject = null;

        InputManager.instance.interactKey.keyPress -= OnDropLarge;
    }

    public void PickupObject(PickupObject obj)
    {
        largeObject = obj;
        //helps with stability
        previousDrag = largeObject.rb.linearDamping;
        largeObject.rb.linearDamping = 5f;

        InputManager.instance.interactKey.keyPress += OnDropLarge;
    }

    public void PickupSmallObject(SmallPickupObject obj)
    {
        smallObject = obj;
        smallObject.transform.position = smallHoldPos.position;
        smallObject.transform.parent = smallHoldPos;
        smallObject.transform.localRotation = Quaternion.identity;

        smallObject.rb.isKinematic = true;

        InputManager.instance.dropKey.keyPress += OnDropSmall;
    }

    public void DropSmallObject()
    {
        smallObject.transform.parent = null;
        smallObject.transform.position = transform.position;

        smallObject.rb.isKinematic = false;

        smallObject = null;

        InputManager.instance.dropKey.keyPress -= OnDropSmall;
    }

    private void OnDropLarge(bool input)
    {
        if(input && largeObject)
        {
            DropObject();
        }
    }

    private void OnDropSmall(bool input)
    {
        if(input && smallObject)
        {
            DropSmallObject();
        }
    }
}