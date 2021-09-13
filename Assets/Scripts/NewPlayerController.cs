﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

enum NewPlayerState { Idle, Running, Jumping, Falling }

[RequireComponent(typeof(CharacterController))]
public class NewPlayerController : MonoBehaviour
{
    [Header("Movement Variables")]
    public float runSpeed = 10.0f;
    public float maxJumpForce = 10;
    public float forceIncrement = 0.4f;

    public float m_JumpHeight = 5.0f;
    public float m_FallMultiplier = 2.5f;
    public float m_LowJumpMultiplier = 2f;
    private bool m_JumpPressed;
    private float m_JumpTimer;
    private float m_JumpGracePeriod = 0.2f;
    private float m_HorizontalInput;
    public bool m_IsGrounded;
    public bool m_Blocked;
    public bool m_IsAlive;
    private Vector3 moveVelocity;
    private Vector3 m_PrevPos;
    private Vector3 m_CurrentVel;
    [SerializeField] private LayerMask m_GroundLayers;
    [SerializeField] private Transform[] m_GroundChecks;
    [SerializeField] private Transform[] m_WallChecks;

    // private variabels
    private float jumpForce = 0;
    const float bottomBoundary = -20;
    NewPlayerState state;

    // Controllers/Managers
    CharacterController controller;
    NewGameManager gameManager;
    Touch touchInput;

    //bool onGround;

    [Header("Events")]
    public UnityEvent onBegin;
    public UnityEvent onJump, onHighJump, onFall, onLand, onCollisionWithWall, onCollisionWithHazard, 
        onFallOffLevel, onCoinCollect;

    // Properties
    public bool Grounded { get => controller.isGrounded; }
    public float Y_Velocity { get => m_CurrentVel.y; }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        gameManager = NewGameManager.m_Instance;
        //onGround = true;
        m_IsAlive = true;

        state = NewPlayerState.Idle;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.State != GameState.Running)
        {
            if (controller.enabled)
                controller.enabled = false;

            return;
        }

        UpdatePosition();
        UpdateState();

        gameManager.SetJumpMeter(jumpForce, maxJumpForce);

        //Debug.Log("On Ground: " + onGround);
    }

    private void LateUpdate()
    {
        if(transform.position.z != 0)
        {
            controller.enabled = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            controller.enabled = true;
        }
    }

    private void UpdatePosition()
    {
        m_HorizontalInput = 1;

        m_IsGrounded = false;
        foreach (var groundCheck in m_GroundChecks)
        {
            if (Physics.CheckSphere(groundCheck.position, 0.1f, m_GroundLayers, QueryTriggerInteraction.Ignore))
            {
                m_IsGrounded = true;
                break;
            }
        }

        if (m_IsGrounded && moveVelocity.y < 0)
        {
            moveVelocity.y = 0f;
        }
        else if(m_IsAlive)
        {
            //Add Gravity
            moveVelocity.y += gameManager.Gravity * Time.deltaTime;
        }
        m_Blocked = false;
        foreach (var wallCheck in m_WallChecks)
        {
            if (Physics.CheckSphere(wallCheck.position, 0.1f, m_GroundLayers, QueryTriggerInteraction.Ignore))
            {
                m_Blocked = true;
                break;
            }
        }

        if (!m_Blocked)
        {
            // Horizontal movement
            controller.Move(new Vector3(m_HorizontalInput * runSpeed, 0, 0) * Time.deltaTime);
        }


        //Jumping
        m_JumpPressed = Input.GetButtonDown("Jump");

        if (m_JumpPressed)
        {
            m_JumpTimer = Time.time;
        }

        if (m_IsGrounded && (m_JumpPressed || (m_JumpTimer > 0 && Time.time < m_JumpTimer + m_JumpGracePeriod)))
        {
            state = NewPlayerState.Jumping;
            onJump.Invoke();

            // add jump invoke here
            moveVelocity.y += Mathf.Sqrt(m_JumpHeight * -2.0f * gameManager.Gravity);
            m_JumpTimer = -1;
        }

        // Calculate current player velocity
        m_CurrentVel = (transform.position - m_PrevPos) / Time.deltaTime;
        m_PrevPos = transform.position;

        // Jump handling
        if (m_CurrentVel.y < 0)
        {
            // If falling
            moveVelocity += (Vector3.up * gameManager.Gravity * (m_FallMultiplier - 1) * Time.deltaTime);
            Debug.Log("Falling");
        }
        else if (m_CurrentVel.y > 0 && !Input.GetButton("Jump"))
        {
            //Low jump multiplier
            moveVelocity += (Vector3.up * gameManager.Gravity * (m_LowJumpMultiplier - 1) * Time.deltaTime);
            Debug.Log("LowJump");
        }

        // Vertical velocity
        controller.Move(moveVelocity * Time.deltaTime);
    }

    private void UpdateState()
    {
        if(state == NewPlayerState.Idle && Input.anyKey)
        {
            Debug.Log("Start running");
            state = NewPlayerState.Running;
        }

        // running -> Falling
        if (state == NewPlayerState.Running && !m_IsGrounded && m_CurrentVel.y < 0)
        {
            state = NewPlayerState.Falling;
            onFall.Invoke();
        }

        // jumping -> falling
        if (state == NewPlayerState.Jumping && m_CurrentVel.y <= 0)
        {
            state = NewPlayerState.Falling;
            onFall.Invoke();
        }
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (gameManager.State != GameState.Running)
            return;

        // player hits side a wall
        if (state == NewPlayerState.Running && hit.gameObject.CompareTag("Wall"))
        {
            //Debug.Log("Collision with wall");

            state = NewPlayerState.Idle;
            onCollisionWithWall.Invoke();
        }

        // check if there is a collision with the ground
        if (state == NewPlayerState.Falling || state == NewPlayerState.Jumping
            && (hit.gameObject.CompareTag("Ground") ||
            hit.gameObject.CompareTag("Platform")))
        {
            //Debug.Log("Collision with ground");

            //onGround = true;
            moveVelocity.y = 0;
            jumpForce = 0;

            state = NewPlayerState.Running;

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
        if (!m_IsAlive)
            return;

        switch(other.tag)
        {
            case "Coin":
                Destroy(other.gameObject);
                onCoinCollect.Invoke();
                break;

            case "KillBox": case "KillZone":
                m_IsAlive = false;
                onFallOffLevel.Invoke();                
                //gameManager.UpdateGameState(GameState.Dead);
                break;
        }

    }

    /// <summary>
    /// Debug Gizmo drawing
    /// </summary>
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (var checks in m_GroundChecks)
        {
            Gizmos.DrawWireSphere(checks.transform.position, 0.1f);
        }
        Gizmos.color = Color.magenta;
        foreach (var checks in m_WallChecks)
        {
            Gizmos.DrawWireSphere(checks.transform.position, 0.1f);
        }
    }


}
