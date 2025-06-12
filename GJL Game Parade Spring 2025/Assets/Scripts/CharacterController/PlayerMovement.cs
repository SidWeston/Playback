using UnityEngine;
using UnityEngine.InputSystem.Layouts;

public class PlayerMovement : MonoBehaviour
{
    //Components
    [SerializeField] private CharacterController characterController;

    //settings
    private float moveSpeed;
    [SerializeField] private float walkSpeed, sprintSpeed, crouchSpeed;
    [SerializeField] private float jumpHeight;

    //ground check and gravity
    private bool grounded;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheckLocation;

    private Vector3 velocity;
    private float gravity = -9.81f;

    //input variables
    private Vector2 moveVector;
    private bool sprinting, crouching;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        InputManager.instance.moveEvent += OnMove;
        InputManager.instance.jumpKey.keyPress += OnJump;
        InputManager.instance.crouchKey.keyPress += OnCrouch;
        InputManager.instance.sprintKey.keyPress += OnSprint;

        moveSpeed = walkSpeed; //assume player always starts off walking
    }

    // Update is called once per frame
    private void Update()
    {
        //apply gravity
        velocity.y += gravity;
        if (grounded)
        {
            velocity.y = -2f;
        }
        characterController.Move(velocity);

        //move player
        Vector3 movement = transform.right * moveVector.x + transform.forward * moveVector.y;
        characterController.Move(movement * moveSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        grounded = Physics.CheckSphere(groundCheckLocation.position, 0.15f, whatIsGround);
    }

    //input functions

    private void OnMove(Vector2 input)
    {
        moveVector = input;
    }

    private void OnJump(bool input)
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void OnSprint(bool input)
    {
        if(input)
        {
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }
    }

    private void OnCrouch(bool input)
    {

    }

    //ghost recording
    public GhostFrame RecordFrame()
    {
        return new GhostFrame
        {
            position = transform.position,
            rotation = transform.rotation,
            isCrouching = crouching,
            timeStamp = Time.time
        };
    }
}