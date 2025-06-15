using UnityEngine;

public abstract class ActivatableObject : MonoBehaviour
{
    public abstract void Activate(GameObject activator);
    public abstract void Deactivate();
}