using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Turret : MonoBehaviour
{
    //targeting settings
    [SerializeField] private GameObject target;
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private float targetRange = 20.0f;

    //firing settings
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint; //probably the end of the barrel
    [SerializeField] private float fireInterval = 0.1f;
    [SerializeField] private float bulletVelocity = 10.0f;
    
    [SerializeField] private GameObject turretBarrel;
    [SerializeField] private float turretHealth = 100.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(FireTurret());
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            turretBarrel.transform.LookAt(target.transform);
        }
    }

    private IEnumerator FireTurret()
    {
        while (target != null)
        {
            GameObject currentBullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            if(currentBullet.TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce(bulletSpawnPoint.forward * bulletVelocity, ForceMode.Impulse);
            }
            yield return new WaitForSeconds(fireInterval);
        }        
    }
}