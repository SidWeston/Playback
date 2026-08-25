using UnityEngine;
using UnityEngine.Events;

public class ProximityTrigger : MonoBehaviour
{
    [SerializeField] private bool triggerOnce = false;

    [Header("Events")]
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerStay;
    public UnityEvent onTriggerExit;

    //independent trackers for each activation method - enter, stay, exit.
    private bool enterTriggered = false;
    private bool stayTriggered = false;
    private bool exitTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        if(triggerOnce && enterTriggered)
        {
            return;
        }

        enterTriggered = true;

        onTriggerEnter?.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        if (triggerOnce && stayTriggered)
        {
            return;
        }

        stayTriggered = true;

        onTriggerStay?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        if (triggerOnce && exitTriggered)
        {
            return;
        }

        exitTriggered = true;

        onTriggerExit?.Invoke();
    }
}