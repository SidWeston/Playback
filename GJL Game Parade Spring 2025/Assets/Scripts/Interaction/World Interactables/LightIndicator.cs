using UnityEngine;

public class LightIndicator : ActivatableObject
{
    public Material onMaterial;
    public Material offMaterial;

    private Renderer render;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        render = GetComponent<Renderer>();
    }

    public override void Activate(GameObject activator)
    {
        render.material = onMaterial;
    }

    public override void Deactivate()
    {
        render.material = offMaterial;   
    }
}