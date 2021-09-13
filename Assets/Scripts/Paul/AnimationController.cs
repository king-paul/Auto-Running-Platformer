using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private NewPlayerController player;
    private NewGameManager gameManager;
    bool falling;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<NewPlayerController>();
        gameManager = GameObject.FindWithTag("GameController").GetComponent<NewGameManager>();
    }

    private void Update()
    {
        if (falling)
        {
            animator.SetFloat("Y_Velocity", player.Y_Velocity + gameManager.Gravity);
            animator.SetBool("Grounded", player.Grounded);
        }
    }

    public void SetAnimation(string trigger)
    {
        animator.SetTrigger(trigger);
    }

    public void PlayRunningAnimation()
    {
        animator.SetBool("GameRunning", true);
        animator.SetBool("WallCollision", false);
        animator.SetBool("Grounded", true);

        falling = false;
    }

    public void PlayDeadAnimation()
    {
        // play death animation once
        animator.SetBool("Death", true);
        falling = false;
    }

    public void PlayJumpSequence()
    {
        animator.SetTrigger("Jump");
        animator.SetBool("HighJump", false);
        falling = true;
    }

    public void PlayHighJumpSequence()
    {
        animator.SetTrigger("Jump");
        animator.SetBool("HighJump", true);
        falling = true;
    }

    public void PlayIdleState()
    {
        animator.SetBool("WallCollision", true);
        falling = false;
    }

    public void PlayFallAnimation()
    {
        falling = true;
    }

}
