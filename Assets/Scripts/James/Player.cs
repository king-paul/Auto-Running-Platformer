using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: James Kemeny

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask m_GroundLayers;
    [SerializeField] private Transform[] m_GroundChecks;
    [SerializeField] private Transform[] m_WallChecks;
    private CharacterController m_Controller;

    [Header("Player Settings")]
    //public GameObject m_GroundCheckNode;
    public float m_MoveSpeed = 10.0f;
    public float m_JumpHeight = 5.0f;
    public float m_FallMultiplier = 2.5f;
    public float m_LowJumpMultiplier = 2f;
    public float m_Gravity = -9.81f;
    private Vector3 m_Velocity;
    private float m_HorizontalInput;

    public bool m_IsGrounded;
    public bool m_Blocked;
    public bool m_IsAlive;
    private bool m_IsRunning;
    private bool m_JumpPressed;
    private float m_JumpTimer;
    private float m_JumpGracePeriod = 0.2f;

    /// <summary>
    /// Subscribe the player to the GM state change event on awake
    /// </summary>
    public void OnAwake()
    {
        TestGameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    /// <summary>
    /// Unsubscribe when player is destroyed
    /// </summary>
    public void OnDestroy()
    {
        TestGameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    /// <summary>
    /// Update the player's current state when it changes in the Game Manager
    /// </summary>
    /// <param name="_state"> The current game state </param>
    private void GameManagerOnGameStateChanged(GameState _state)
    {
        m_IsRunning = (_state == GameState.Running);

        Debug.Log(_state);

    }

    /// <summary>
    /// 
    /// </summary>
    public void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        m_IsAlive = true;
    }

    public void Update()
    {
        m_IsRunning = (TestGameManager.m_Instance.m_State == GameState.Running);
        if (m_IsRunning)
        {
            m_HorizontalInput = 1;

            transform.forward = new Vector3(m_HorizontalInput, 0, Mathf.Abs(m_HorizontalInput) - 1);

            m_IsGrounded = false;
            foreach (var groundCheck in m_GroundChecks)
            {
                if(Physics.CheckSphere(groundCheck.position, 0.1f, m_GroundLayers, QueryTriggerInteraction.Ignore))
                {
                    m_IsGrounded = true;
                    break;
                }
            }

            if (m_IsGrounded && m_Velocity.y < 0)
            {
                m_Velocity.y = 0f;
            }
            else
            {
                //Add Gravity
                m_Velocity.y += m_Gravity * Time.deltaTime;
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

            if( !m_Blocked)
            {
                // Horizontal movement
                m_Controller.Move(new Vector3(m_HorizontalInput * m_MoveSpeed, 0, 0) * Time.deltaTime);
            }

            //Jumping
            m_JumpPressed = Input.GetButtonDown("Jump");

            if(m_JumpPressed)
            {
                m_JumpTimer = Time.time;
            }

            if (m_IsGrounded && (m_JumpPressed || (m_JumpTimer > 0 && Time.time < m_JumpTimer + m_JumpGracePeriod)))
            {
                
                m_Velocity.y += Mathf.Sqrt(m_JumpHeight * -2.0f * m_Gravity);
                m_JumpTimer = -1;

            }

            // Jump handling
            //if (m_Controller.velocity.y < 0)
            //{
            //    // If falling
            //    m_Velocity.y += m_Gravity * (m_FallMultiplier - 1);
            //    //m_Velocity += (Vector3.up * m_Gravity * (m_FallMultiplier - 1) * Time.deltaTime);
            //    Debug.Log("Falling");
            //}
            //else if (m_Controller.velocity.y > 0 && !Input.GetButton("Jump"))
            //{
            //    //Low jump multiplier
            //    m_Velocity.y += m_Gravity * (m_LowJumpMultiplier - 1);
            //    //m_Velocity += (Vector3.up * m_Gravity * (m_LowJumpMultiplier - 1) * Time.deltaTime);
            //    Debug.Log("LowJump");
            //}

            // Vertical velocity
            m_Controller.Move(m_Velocity * Time.deltaTime);
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "KillBox")
        {
            TestGameManager.m_Instance.UpdateGameState(GameState.Dead);

            m_IsRunning = false;
            m_IsAlive = false;

            Debug.Log("Killed by " + other.gameObject.tag);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            TestGameManager.m_Instance.UpdateGameState(GameState.Dead);

            //Debug.Log("dead");
            m_IsRunning = false;
            m_IsAlive = false;

            Debug.Log("Killed by " + collision.gameObject.tag);
        }
    }

    public void OnCollisionStay(Collision collision)
    {
  
        //if (collision.contacts[0].normal.y > 0.7f)
        //{//check if ground is walkable, in this case 45 degrees and lower
        //    m_GroundNormal = collision.contacts[0].normal;//set up direction
        //}
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        foreach(var checks in m_GroundChecks)
        {
            Gizmos.DrawWireSphere(checks.transform.position,  0.1f);
        }
        Gizmos.color = Color.green;
        foreach (var checks in m_WallChecks)
        {
            Gizmos.DrawWireSphere(checks.transform.position, 0.1f);
        }

    }
}
