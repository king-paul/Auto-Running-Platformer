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
    public float jumpForce;
    //public float gravityModifier;
    public float runSpeed = 10.0f;

    public bool moveCharacter = true;

    // private variabels    
    private int timesJumped = 0;
    private Vector3 moveVelocity;

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
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.GameRunning)
        {
            return;
        }

        if (controller.isGrounded && moveVelocity.y < 0f)
        {
            moveVelocity.y = 0f;
        }

        // if move character is turned onmove object to the right
        if (moveCharacter)
        {
            //transform.position += Vector3.right * runSpeed * Time.deltaTime;
            moveVelocity.x = Vector3.right.x * runSpeed;            
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (controller.isGrounded || timesJumped < 2)
            {
                //controller.Move(Vector3.up * jumpForce * Time.deltaTime);
                //playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                // move character up
                moveVelocity.y += Mathf.Sqrt(jumpForce * -3.0f * gameManager.Gravity);

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

        // make character fall when off ground
        moveVelocity.y += gameManager.Gravity * Time.deltaTime;

        // move the character
        controller.Move(moveVelocity * Time.deltaTime);

        playerAnim.SetFloat("Y_Velocity", moveVelocity.y + gameManager.Gravity);

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!gameManager.GameRunning)
            return;

        // check if there is a collision with the ground
        if (hit.gameObject.CompareTag("Ground") && timesJumped > 0) { 
            timesJumped = 0;

            if(landSound != null)
                playerAudio.PlayOneShot(landSound, 1.5f);

            playerAnim.SetBool("Grounded", true);
        } 
        // check if there is a collision with an obstacle
        else if (hit.gameObject.CompareTag("Obstacle"))
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
