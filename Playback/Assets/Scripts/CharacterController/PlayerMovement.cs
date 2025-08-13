using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Components
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera playerCamera;

    //settings
    private float moveSpeed;
    [SerializeField] private float walkSpeed, sprintSpeed, crouchSpeed;
    [SerializeField] private float jumpHeight;

    [SerializeField] private Vector3 stoodCamPos, crouchedCamPos;

    //ground check and gravity
    public bool grounded;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckLocation;

    private Vector3 velocity;
    private float gravity = -9.81f;

    //input variables
    private Vector2 moveVector;
    private bool sprinting, crouching;
    private bool interacting = false;

    //events for animations
    public event Action<bool> landedEvent;
    public event Action<bool> jumpEvent;

    private Vector3 startPosition;

    public bool movementEnabled = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        InputManager.instance.moveEvent += OnMove;
        InputManager.instance.jumpKey.keyPress += OnJump;
        InputManager.instance.crouchKey.keyPress += OnCrouch;
        InputManager.instance.sprintKey.keyPress += OnSprint;
        InputManager.instance.interactKey.keyPress += OnInteract;

        moveSpeed = walkSpeed; //assume player always starts off walking

        startPosition = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!movementEnabled) return;

        GroundCheck();
        //apply gravity
        velocity.y += gravity * Time.deltaTime;
        if (grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //move player
        //movement direction based on inputs
        Vector3 movementInput = transform.right * moveVector.x + transform.forward * moveVector.y;
        //add gravity to it
        Vector3 totalMovement = movementInput * moveSpeed + Vector3.up * velocity.y;
        characterController.Move(totalMovement * Time.deltaTime);

        if (transform.position.y < -15f)
        {
            transform.position = startPosition; velocity.y = 0;
        }
    }

    private void GroundCheck()
    {
        bool checkGround = Physics.CheckSphere(groundCheckLocation.position, 0.3f, groundLayer);
        if(checkGround && grounded != checkGround)
        {
            landedEvent?.Invoke(true);
        }
        grounded = checkGround;
    }

    //input functions
    private void OnMove(Vector2 input)
    {
        moveVector = input;
    }

    private void OnJump(bool input)
    {
        if(input && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpEvent?.Invoke(true);
        }
    }

    private void OnSprint(bool input)
    {
        if(input)
        {
            moveSpeed = sprintSpeed;
            sprinting = true;
        }
        else
        {
            moveSpeed = walkSpeed;
            sprinting = false;
        }
    }

    private void OnCrouch(bool input)
    {
        if(input)
        {
            crouching = true;
            playerCamera.transform.localPosition = crouchedCamPos;
            characterController.height = 1f;
            characterController.center = new Vector3(0, -0.5f, 0);
        }
        else
        {
            crouching = false;
            playerCamera.transform.localPosition = stoodCamPos;
            characterController.height = 2f;
            characterController.center = Vector3.zero;
        }
    }

    private void OnInteract(bool input)
    {
        interacting = input;
    }

    //2 seperate frame recording structs, probably messy but who cares

    //ghost recording
    public GhostFrame RecordFrame()
    {
        return new GhostFrame
        {
            position = transform.position,
            cameraForward = playerCamera.transform.forward,
            rotation = transform.rotation,
            movementInput = moveVector,
            isCrouching = crouching,
            isSprinting = sprinting,
            isJumping = grounded
        };
    }

    //player rewind recording
    public PlayerFrame RecordRewindFrame()
    {
        return new PlayerFrame
        {
            position = transform.position,
            rotation = transform.rotation,
            isCrouching = crouching,
            timeStamp = Time.time
        };
    }

}