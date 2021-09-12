using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

enum PlayerState { Idle, Running, Jumping, Falling }

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // public variables
    [Header("Movement Variables")]
    public float runSpeed = 10.0f;
    public float maxJumpForce = 10;
    public float forceIncrement = 0.4f;

    // private variabels
    private float jumpForce = 0;
    private Vector3 moveVelocity;
    const float bottomBoundary = -20;
    PlayerState state;

    // Controllers/Managers
    CharacterController controller;
    GameManager gameManager;
    Touch touchInput;

    //bool onGround;

    [Header("Events")]
    public UnityEvent onBegin;
    public UnityEvent onJump, onHighJump, onFall, onLand, onCollisionWithWall, onCollisionWithHazard, 
        onFallOffLevel, onCoinCollect;

    // Properties
    public bool Grounded { get => controller.isGrounded; }
    public float Y_Velocity { get => moveVelocity.y; }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //onGround = true;

        state = PlayerState.Running;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.State != GameState.Running)
        {
            controller.enabled = false;
            return;
        }
        else if(!controller.enabled)
        {
            controller.enabled = true;
        }

        HandleInput();
        UpdatePosition();
        UpdateState();

        gameManager.SetJumpMeter(jumpForce, maxJumpForce);

        //Debug.Log("On Ground: " + onGround);
    }

    private void UpdatePosition()
    {
        // make character fall when off ground
        if (!controller.isGrounded)
        {
            moveVelocity.y += gameManager.Gravity * Time.deltaTime;
            //onGround = false;
        }
        // otherwise reset the y component of the velocity vector
        else if (moveVelocity.y < 0f)
            moveVelocity.y = 0f;

        //transform.position += Vector3.right * runSpeed * Time.deltaTime;
        moveVelocity.x = Vector3.right.x * runSpeed;        

        // move the character
        controller.Move(moveVelocity * Time.deltaTime);
    }

    private void HandleInput()
    {
        // ensure the the player is running all standing
        if (state != PlayerState.Running && state != PlayerState.Idle)
            return;

        if (Input.touchCount > 0)
            touchInput = Input.GetTouch(0);

        // build up jump force while jump is held down
        if (Input.GetKey(KeyCode.Space) || touchInput.phase == TouchPhase.Stationary)
        {
            if (jumpForce < maxJumpForce)
            {
                jumpForce += forceIncrement;
            }

            if (jumpForce > maxJumpForce)
                jumpForce = maxJumpForce;
        }

        // make the chatacter jump when jump is released
        if (Input.GetKeyUp(KeyCode.Space) || touchInput.phase == TouchPhase.Ended || 
            jumpForce >= maxJumpForce)
        {
            // move character up
            moveVelocity.y += Mathf.Sqrt(jumpForce * -3.0f * gameManager.Gravity);

            if (jumpForce > (maxJumpForce + 1) / 2)
                onHighJump.Invoke();
            else
                onJump.Invoke();

            //onGround = false;

            state = PlayerState.Jumping;
        }

    }

    private void UpdateState()
    {
        // idle -> running
        //if (state == PlayerState.Idle && moveVelocity.x > 0)
            //state = PlayerState.Running;

        // running -> Falling
        if (state == PlayerState.Running && !controller.isGrounded && moveVelocity.y < 0)
        {
            state = PlayerState.Falling;
            onFall.Invoke();
        }

        // jumping -> falling
        if (state == PlayerState.Jumping && moveVelocity.y <= 0)
            state = PlayerState.Falling;

        // check if the character has fallen below the boundary
        if (transform.position.y < bottomBoundary)        
            onFallOffLevel.Invoke();   
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (gameManager.State != GameState.Running)
            return;

        // player hits side a wall
        if (state == PlayerState.Running && hit.gameObject.CompareTag("Wall"))
        {
            //Debug.Log("Collision with wall");

            state = PlayerState.Idle;
            onCollisionWithWall.Invoke();
        }

        // check if there is a collision with the ground
        if (state == PlayerState.Falling || state == PlayerState.Jumping
            && (hit.gameObject.CompareTag("Ground") ||
            hit.gameObject.CompareTag("Platform")))
        {
            //Debug.Log("Collision with ground");

            //onGround = true;
            moveVelocity.y = 0;
            jumpForce = 0;

            state = PlayerState.Running;

            onLand.Invoke();
        }

        // check if there is a collision with an obstacle
        if (hit.gameObject.CompareTag("Obstacle") || hit.gameObject.CompareTag("Hazard") ||
            hit.gameObject.CompareTag("Arrow"))
        {
            onCollisionWithHazard.Invoke();

            //gameManager.EndGame();
            gameManager.UpdateGameState(GameState.Dead);
        }

    }

    // Trigger collisions
    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "Coin":
                Destroy(other.gameObject);
                onCoinCollect.Invoke();
                break;

            case "KillBox": case "KillZone":
                onFallOffLevel.Invoke();
                break;
        }

    }

}
