using UnityEngine;
using Animancer;

public class PlayerAnimationController : MonoBehaviour
{
    public AnimancerComponent animancer;
    public PlayerMovement playerMovement;

    public DirectionalAnimationSet8 currentMoveSet;
    public AnimationClip currentIdle;

    public AnimationClip jump;

    public AnimSwitcher animSwitcher;

    private Vector2 moveVector;
    private bool crouching;
    private bool sprinting;
    private bool jumping;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.instance.moveEvent += OnMove;
        InputManager.instance.sprintKey.keyPress += OnSprint;
        InputManager.instance.crouchKey.keyPress += OnCrouch;

        playerMovement.jumpEvent += OnJump;
        playerMovement.landedEvent += OnLanded;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMove(Vector2 input)
    {
        moveVector = input;
        if (!playerMovement.grounded) return;
        if(moveVector == Vector2.zero)
        {
            animancer.Play(currentIdle, 0.15f);
        }
        else
        {
            animancer.Play(currentMoveSet.Get(moveVector), 0.15f);
        }
    }

    private void OnSprint(bool input)
    {
        if (!playerMovement.grounded || crouching) return;

        if(input)
        {
            sprinting = true;
            currentMoveSet = animSwitcher.sprintSet;

            if(moveVector != Vector2.zero)
            {
                animancer.Play(currentMoveSet.Get(moveVector), 0.15f);
            }
        }
        else
        {
            sprinting = false;
            currentMoveSet = animSwitcher.walkSet;

            if (moveVector != Vector2.zero)
            {
                animancer.Play(currentMoveSet.Get(moveVector), 0.15f);
            }
        }
    }

    private void OnCrouch(bool input)
    {
        if (!playerMovement.grounded || sprinting) return;

        if(input)
        {
            crouching = true;
            currentMoveSet = animSwitcher.crouchSet;
            currentIdle = animSwitcher.crouchIdle;

            if (moveVector == Vector2.zero)
            {
                animancer.Play(currentIdle, 0.15f);
            }
            else
            {
                animancer.Play(currentMoveSet.Get(moveVector), 0.15f);
            }
        }
        else
        {
            crouching = false;
            currentMoveSet = animSwitcher.walkSet;
            currentIdle = animSwitcher.idle;
            if (moveVector == Vector2.zero)
            {
                animancer.Play(currentIdle, 0.15f);
            }
            else
            {
                animancer.Play(currentMoveSet.Get(moveVector), 0.15f);
            }
        }
    }

    private void OnJump(bool input)
    {
        if(input)
        {
            animancer.Play(jump);

            if (crouching)
            {
                currentMoveSet = animSwitcher.walkSet;
                currentIdle = animSwitcher.idle;
            }
        }
    }

    private void OnLanded(bool landed)
    {
        if(landed)
        {
            if(moveVector != Vector2.zero)
            {
                animancer.Play(currentMoveSet.Get(moveVector), 0.15f);
            }
            else
            {
                animancer.Play(currentIdle, 0.15f);
            }
        }
    }
}
