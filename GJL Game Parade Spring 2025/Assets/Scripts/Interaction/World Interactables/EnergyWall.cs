using UnityEngine;

public class EnergyWall : MonoBehaviour
{

    [SerializeField] private LayerMask ghostLayer;

    [SerializeField] private GameObject explodeVFXPrefab;
    [SerializeField] private float vfxDuration = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if((ghostLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            if(other.gameObject.TryGetComponent(out GhostPlayer ghost))
            {
                GameObject vfx = Instantiate(explodeVFXPrefab, ghost.transform.position, ghost.transform.rotation);
                Destroy(vfx, vfxDuration);
                ghost.ToggleGhost(true);
                if(gameObject.TryGetComponent(out Renderer renderer))
                {
                    renderer.material.SetFloat("_GlitchAmount", 0.5f);
                    Invoke("ResetGlitch", 0.5f);
                }
                GameUI.instance.SetPauseSymbol();
                GameUI.instance.CancelRecordTime();
            }
        }
    }

    private void ResetGlitch()
    {
        if (gameObject.TryGetComponent(out Renderer renderer))
        {
            renderer.material.SetFloat("_GlitchAmount", 0);
        }
    }
}