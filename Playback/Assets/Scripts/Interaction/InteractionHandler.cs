using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    //components
    [SerializeField] private Camera playerCamera;

    //settings
    [SerializeField] private float range;
    [SerializeField] private LayerMask interactableLayers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.instance.interactKey.keyPress += OnInteract;
    }

    private void OnInteract(bool input)
    {
        if(input)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range, interactableLayers))
            {
                if (hit.collider.gameObject.TryGetComponent(out Interactable interactable))
                {
                    interactable.Interact(gameObject);
                }
            }
        }
    }
}