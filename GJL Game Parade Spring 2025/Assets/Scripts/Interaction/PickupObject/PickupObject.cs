using UnityEngine;

public class PickupObject : MonoBehaviour, Interactable
{


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