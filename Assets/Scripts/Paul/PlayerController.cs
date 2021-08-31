using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    // public variables
    public float jumpForce;
    public float gravityModifier;
    public float runSpeed = 10.0f;

    public bool moveCharacter = true;

    // private variabels    
    private int timesJumped = 0;
    private bool isOnGround = true;

    // Controllers/Managers
    CharacterController controller;
    GameManager gameManager;

    // animation objects
    private Rigidbody playerRb;
    private Animator playerAnim;
    public ParticleSystem explosionParticle;

    // audio objects
    public AudioClip jumpSound;
    public AudioClip deathSound;
    public AudioClip landSound;
    private AudioSource playerAudio;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        Physics.gravity *= gravityModifier;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.GameRunning)
        {
            return;                       
        }

        //playerAnim.enabled = true;
        playerAnim.SetFloat("Speed", 1);
        playerAnim.SetFloat("Y_Velocity", playerRb.velocity.y);

        // if move character is turned onmove object to the right
        if (moveCharacter)
        {
            transform.position += Vector3.right * runSpeed * Time.deltaTime;
            //controller.Move(Vector3.right * runSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isOnGround || timesJumped < 2)
            {
                //controller.Move(Vector3.up * jumpForce * Time.deltaTime);

                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isOnGround = false;

                if (timesJumped == 0)
                    playerAnim.SetTrigger("Jump");
                else
                    playerAnim.SetTrigger("Double_Jump");

                playerAnim.SetBool("Grounded", false);
                playerAudio.PlayOneShot(jumpSound, 1.0f);

                timesJumped++;
            }
        }

        // check if the character has fallen off the building
        if(transform.position.y < -10)
        {
            gameManager.EndGame();
            playerAnim.enabled = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        // check if there is a collision with the ground
        if (collision.gameObject.CompareTag("Ground") && gameManager.GameRunning) { 
            isOnGround = true;
            timesJumped = 0;

            if(landSound != null)
                playerAudio.PlayOneShot(landSound, 1.5f);

            playerAnim.SetBool("Grounded", true);
        } 
        // check if there is a collision with an obstacle
        else if (collision.gameObject.CompareTag("Obstacle") && gameManager.GameRunning)
        {
            // play death animation once
            playerAnim.SetBool("Death", true);
            playerAnim.SetInteger("DeathType", 1);

            if(explosionParticle != null)
                explosionParticle.Play();

            playerAudio.PlayOneShot(deathSound, 1.0f);
            Debug.Log("Game Over!");

            gameManager.EndGame();
        }

    }

}
