using UnityEngine;
using Animancer;

public class MenuAnimController : MonoBehaviour
{

    public AnimancerComponent animancer;

    public AnimationClip idle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animancer.Play(idle);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
