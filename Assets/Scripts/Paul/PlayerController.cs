using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    // public variables
    public float jumpForce;
    public float gravityModifier;
    
    public float waitTime = 2.0f;
    public float runSpeed = 10.0f;

    public bool moveCharacter = true;

    // private variabels
    private bool isWalking;
    private float timeLeft;
    private int timesJumped = 0;
    private bool isOnGround = true;
    private bool gameOver = false;

    // annimation obkects
    private Rigidbody playerRb;
    private Animator playerAnim;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    // audio objects
    public AudioClip jumpSound;
    public AudioClip crashSound;
    public AudioClip landSound;
    private AudioSource playerAudio;

    // Game State
    public enum State { standing, walking, waiting, running, gameOver};
    private State gameState;

    // Properties
    public bool GameOver { get => gameOver; }
    public State GameState { get => gameState; }

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        Physics.gravity *= gravityModifier;
        timeLeft = waitTime;
        gameState = State.running;
    }

    // Update is called once per frame
    void Update()
    {
        // call appropriate method based on the state the game is in
        switch(gameState)
        {
            case State.standing: Stand();
                break;
            case State.walking: Walk();
                break;
            case State.waiting: Wait();
                break;
            case State.running: Run();
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // check if there is a collision with the ground
        if (collision.gameObject.CompareTag("Ground") && gameState == State.running) { 
            isOnGround = true;
            timesJumped = 0;

            if (dirtParticle != null)
                dirtParticle.Play();

            if(landSound != null)
                playerAudio.PlayOneShot(landSound, 1.5f);

            playerAnim.SetBool("Grounded", true);
        } 
        // check if there is a collision with an obstacle
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameOver = true;            

            // play death animation once
            playerAnim.SetBool("Death", true);
            playerAnim.SetInteger("DeathType", 1);

            // change the particle system to the explosion
            if (dirtParticle != null)
                dirtParticle.Stop();

            if(explosionParticle != null)
                explosionParticle.Play();

            playerAudio.PlayOneShot(crashSound, 1.0f);
            Debug.Log("Game Over!");
        }

    }

    private void Walk()
    {
        playerAnim.enabled = true;

        if (transform.position.x < 0)
            transform.Translate(0, 0, 0.1f);
        else
            gameState = State.waiting;
    }

    private void Run()
    {
        playerAnim.enabled = true;
        playerAnim.SetFloat("Speed", 1);
        playerAnim.SetFloat("Y_Velocity", playerRb.velocity.y);

        // if move character is turned onmove object to the right
        if (moveCharacter)
        {
            //playerRb.velocity = Vector3.right * runSpeed;
            //playerRb.AddForce(Vector3.right * runSpeed, ForceMode.Force);
            //Debug.Log("Velocity" + playerRb.velocity);
            transform.position += Vector3.right * runSpeed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !gameOver)
        { 
            if (isOnGround || timesJumped < 2)
            {          
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isOnGround = false;

                if (timesJumped == 0)
                    playerAnim.SetTrigger("Jump");
                else
                    playerAnim.SetTrigger("Double_Jump");

                playerAnim.SetBool("Grounded", false);

                if (dirtParticle != null)
                    dirtParticle.Stop();

                playerAudio.PlayOneShot(jumpSound, 1.0f);

                timesJumped++;      
            }
        }

    }

    private void Stand()
    {
        playerAnim.enabled = false;

        if(dirtParticle != null)
            dirtParticle.Stop();

        if (timeLeft > 0)
            timeLeft -= Time.deltaTime;
        else
        {
            gameState = State.walking;
            timeLeft = waitTime;
        }
    }

    private void Wait ()
    {
        playerAnim.enabled = false;

        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else
        {
            gameState = State.running;
            timeLeft = waitTime;

            if (dirtParticle != null)
                dirtParticle.Play();
        }
    }

}
