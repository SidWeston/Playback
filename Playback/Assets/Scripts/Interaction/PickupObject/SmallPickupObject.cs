using UnityEngine;

public class SmallPickupObject : MonoBehaviour, Interactable
{
    public Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact(GameObject interactor)
    {
        if (interactor.TryGetComponent(out ObjectHolder holder))
        {
            if (!holder.largeObject && !holder.smallObject)
            {
                holder.PickupSmallObject(this);
            }
        }
    }
}