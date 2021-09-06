using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    // public variables
    [Header("Movement Variables")]
    public float runSpeed = 10.0f;
    public bool moveCharacter = true;
    public float maxJumpForce = 10;
    public float forceIncrement = 0.4f;    

    // private variabels
    private float jumpForce = 0;
    private Vector3 moveVelocity;
    const float bottomBoundary = -20;    

    // Controllers/Managers
    CharacterController controller;
    GameManager gameManager;
    Touch touchInput;

    // animation objects
    private Rigidbody playerRb;
    private Animator playerAnim;
    bool onGround;

    // audio objects
    [Header("Audio Clips")]
    public AudioClip jumpSound;
    public AudioClip highJumpSound;
    public AudioClip landSound;
    public AudioClip collideSound;
    public AudioClip fallSound;    
    private AudioSource playerAudio;
    private AudioSource footsteps;
    private bool alive;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();

        // get audio sources
        var audioSources = GetComponents<AudioSource>();
        footsteps = audioSources[0];
        playerAudio = audioSources[1];   

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        footsteps.Play();
        onGround = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.State != GameState.Running)
        {
            if (footsteps.isPlaying)
                footsteps.Stop();

            return;
        }

        HandleInput();
        UpdatePosition();

        // check if the player is still on the ground
        if (!controller.isGrounded)
        {
            if(onGround)
                onGround = false;
            if (footsteps.isPlaying)
                footsteps.Stop();
        }            

        // check if the character has fallen below the boundary
        if (transform.position.y < bottomBoundary)
        {
            playerAnim.enabled = false;
            playerAudio.PlayOneShot(fallSound);
            gameManager.UpdateGameState(GameState.Dead);
        }

        // update animation parameters
        playerAnim.SetFloat("Y_Velocity", moveVelocity.y + gameManager.Gravity);
        playerAnim.SetBool("Grounded", controller.isGrounded);

        gameManager.SetJumpMeter(jumpForce, maxJumpForce);       
    }

    private void UpdatePosition()
    {
        // make character fall when off ground
        if (!controller.isGrounded)
            moveVelocity.y += gameManager.Gravity * Time.deltaTime;
        // otherwise reset the y component of the velocity vector
        else if(moveVelocity.y < 0f)
            moveVelocity.y = 0f;

        // if move character is turned onmove object to the right
        if (moveCharacter)
        {
            //transform.position += Vector3.right * runSpeed * Time.deltaTime;
            moveVelocity.x = Vector3.right.x * runSpeed;
        }

        // move the character
        controller.Move(moveVelocity * Time.deltaTime);
    }
     
    private void HandleInput()
    {
        if(Input.touchCount > 0)
            touchInput = Input.GetTouch(0);

        // build up jump force while jump is held down
        if ((Input.GetKey(KeyCode.Space) || touchInput.phase == TouchPhase.Stationary) &&
            onGround)
        {
            if (jumpForce < maxJumpForce)
            {
                jumpForce += forceIncrement;
            }

             if (jumpForce > maxJumpForce)
                jumpForce = maxJumpForce;   
        }

        // make the chatacter jump when jump is released
        if ((Input.GetKeyUp(KeyCode.Space) || touchInput.phase == TouchPhase.Ended ||
            jumpForce >= maxJumpForce) && onGround)
        {
            // move character up
            moveVelocity.y += Mathf.Sqrt(jumpForce * -3.0f * gameManager.Gravity);

            // Update animation
            playerAnim.SetFloat("JumpPower", jumpForce);
            playerAnim.SetTrigger("Jump");
            // Turn off wall collision
            playerAnim.SetBool("WallCollision", false);

            if (jumpForce > (maxJumpForce+1) / 2)
                playerAudio.PlayOneShot(highJumpSound, 1.0f);
            else
                playerAudio.PlayOneShot(jumpSound, 1.0f);

              onGround = false;
        }

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (gameManager.State != GameState.Running)
            return;

        // check if there is a collision with the ground
        if (!onGround && (hit.gameObject.CompareTag("Ground") || 
            hit.gameObject.CompareTag("Platform")))
        {
            // player lands on top of object
            //if (transform.position.y > hit.transform.position.y)
            //{
                onGround = true;                
                moveVelocity.y = 0;
                jumpForce = 0;

                if (landSound != null && !playerAudio.isPlaying)
                    playerAudio.PlayOneShot(landSound, 1.5f);

                footsteps.PlayScheduled(2);
                playerAnim.SetBool("Grounded", true);
            //}
        }

        // player hits side of object
        if (hit.gameObject.CompareTag("Wall"))
        {
            playerAnim.SetBool("WallCollision", true);
        }

        // check if there is a collision with an obstacle
        if (hit.gameObject.CompareTag("Obstacle") || hit.gameObject.CompareTag("Hazard") ||
            hit.gameObject.CompareTag("Arrow"))
        {
            // play death animation once
            playerAnim.SetBool("Death", true);
            playerAnim.SetInteger("DeathType", 1);

            playerAudio.PlayOneShot(collideSound, 1.0f);
            Debug.Log("Game Over!");

            //gameManager.EndGame();
            gameManager.UpdateGameState(GameState.Dead);
        }

    }

}
