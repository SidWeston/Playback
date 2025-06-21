using UnityEngine;

public class PickupObject : MonoBehaviour, Interactable
{

    private Vector3 startPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= -15f)
        {
            transform.position = startPosition;
        }
    }

    public void Interact(GameObject interactor)
    {
        if(interactor.TryGetComponent(out ObjectHolder holder))
        {
            if(holder.currentObject == this)
            {
                //drop it
                holder.DropObject();
            }
            else
            {
                //pick it up
                holder.PickupObject(this);
            }
        }
    }
}