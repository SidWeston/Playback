using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [SerializeField] private LayerMask destroyWhenCollide;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if((destroyWhenCollide.value & (1 << collision.gameObject.layer)) != 0)
        {
            if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Ghost")
            {
                
            }
        }        
    }

}