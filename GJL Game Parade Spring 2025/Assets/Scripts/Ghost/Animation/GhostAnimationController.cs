using UnityEngine;
using Animancer;

public class GhostAnimationController : MonoBehaviour
{
    public AnimancerComponent animancer;

    private DirectionalAnimationSet8 currentMoveSet;
    private AnimationClip currentIdle;

    public AnimSwitcher animSwitcher;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentIdle = animSwitcher.idle;
        currentMoveSet = animSwitcher.walkSet;
        animancer.Play(currentIdle); //assume it starts idle
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMovementAnimation(Vector2 moveVector)
    {
        if (moveVector == Vector2.zero)
        {
            animancer.Play(currentIdle, 0.15f);
        }
        else
        {
            animancer.Play(currentMoveSet.Get(moveVector), 0.15f);
        }
    }

    public void SwitchAnimSet(bool crouching, bool sprinting)
    {
        if(crouching)
        {
            currentMoveSet = animSwitcher.crouchSet;
            currentIdle = animSwitcher.crouchIdle;
            return;
        }

        if(sprinting)
        {
            currentMoveSet = animSwitcher.sprintSet;
            currentIdle = animSwitcher.idle;
            return;
        }

        currentMoveSet = animSwitcher.walkSet;
        currentIdle = animSwitcher.idle;
    }
}