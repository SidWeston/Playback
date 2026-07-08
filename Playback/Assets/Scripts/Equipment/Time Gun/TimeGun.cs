using UnityEngine;

public class TimeGun : MonoBehaviour
{
    public bool canFire = true;

    [SerializeField] private Camera playerCam;
    [SerializeField] private float range = 100.0f;
    [SerializeField] private LayerMask timeLayers;

    [SerializeField] private ParticleSystem muzzleFlash;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.instance.shootKey += OnShoot;
    }

    private void OnDestroy()
    {
        InputManager.instance.shootKey -= OnShoot;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnShoot(bool input)
    {
        if(input && canFire)
        {
            muzzleFlash.Play();
            Invoke(nameof(StopMuzzleFlash), 0.2f);

            RaycastHit hit;

            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, range, timeLayers))
            {
                GameObject go = hit.collider.gameObject;
                if(go.TryGetComponent(out TimeStop time))
                {
                    if(time.stopped)
                    {
                        time.StartTime();
                    }
                    else
                    {
                        time.StopTime();
                    }
                }
            }
        }
    }

    private void StopMuzzleFlash()
    {
        muzzleFlash.Stop();
    }
}
