using UnityEngine;

public class ButtonBase : MonoBehaviour
{
    public bool active = false;

    [SerializeField] protected LineRenderer powerline;
    [SerializeField] protected Material lineOff, lineOn;
    //powerlight
    [SerializeField] protected Renderer wallLight;
    [SerializeField] protected Material lightOff, lightOn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
