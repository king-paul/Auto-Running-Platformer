using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class PlayerControllerTest : MonoBehaviour
{
    [Header("Collision Checks")]
    [SerializeField] private LayerMask m_GroundLayers = default;
    [SerializeField] private Transform[] m_GroundChecks = null;
    [SerializeField] private Transform[] m_WallChecks = null;
    
    [Header("Player Settings")]
    //public GameObject m_GroundCheckNode;
    public float m_MoveSpeed = 10.0f;
    public float m_JumpHeight = 5.0f;
    public float m_FallMultiplier = 2.5f;
    public float m_LowJumpMultiplier = 2f;    

    public UnityEvent onStart, onJump, onFall, onLand, onCollisionWithWall, onCollisionWithHazrd;

    // private variables/objects
    private CharacterController m_Controller;
    private GameManager gameManager;

    private float m_Gravity; // obtained from game manager

    private Vector3 m_Velocity;
    private Vector3 m_Prev;
    private Vector3 m_CurrentVel;
    private float m_HorizontalInput;

    private bool m_IsGrounded;
    private bool m_Blocked;
    private bool m_IsAlive = false;
    private bool m_IsRunning;
    private bool m_JumpPressed = false;
    private float m_JumpTimer;
    private float m_JumpGracePeriod = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();

        m_IsAlive = true;
        m_Gravity = gameManager.Gravity;
    }

    // Update is called once per frame
    void Update()
    {
        m_IsRunning = (gameManager.State == GameState.Running);

        if (m_IsRunning)
        {
            m_IsGrounded = false;
            m_Blocked = false;

            PerformWallChecks();
            UpdateForwardMovement();
            UpdateVerticalMovement();

            // Calculate current player velocity
            m_CurrentVel = (transform.position - m_Prev) / Time.deltaTime;
            m_Prev = transform.position;

            // move character
            m_Controller.Move(m_Velocity * Time.deltaTime);
        }
    }

    private void UpdateForwardMovement()
    {
        m_HorizontalInput = 1;
        transform.forward = new Vector3(m_HorizontalInput, 0, Mathf.Abs(m_HorizontalInput) - 1);

        if (!m_Blocked)
        {
            // Horizontal movement
            m_Controller.Move(new Vector3(m_HorizontalInput * m_MoveSpeed, 0, 0) * Time.deltaTime);
        }
    }

    private void UpdateVerticalMovement()
    {
        if (Input.GetButtonDown("Jump"))
        {
            m_JumpTimer = Time.time;
        }

        if (m_IsGrounded && (m_JumpPressed || (m_JumpTimer > 0 && Time.time < m_JumpTimer + m_JumpGracePeriod)))
        {
            m_Velocity.y += Mathf.Sqrt(m_JumpHeight * -2.0f * m_Gravity);
            m_JumpTimer = -1;
        }

        // Apply gravity
        if (m_IsGrounded && m_Velocity.y < 0)
        {
            m_Velocity.y = 0f;
        }
        else
        {
            //Add Gravity
            m_Velocity.y += m_Gravity * Time.deltaTime;
        }

        // Jump handling
        if (m_CurrentVel.y < 0)
        {
            // If falling
            //m_Velocity.y += m_Gravity * (m_FallMultiplier - 1);
            m_Velocity += (Vector3.up * m_Gravity * (m_FallMultiplier - 1) * Time.deltaTime);
            Debug.Log("Falling");
        }
        else if (m_CurrentVel.y > 0 && !Input.GetButton("Jump"))
        {
            //Low jump multiplier
            //m_Velocity.y += m_Gravity * (m_LowJumpMultiplier - 1);
            m_Velocity += (Vector3.up * m_Gravity * (m_LowJumpMultiplier - 1) * Time.deltaTime);
            Debug.Log("LowJump");
        }
    }

    private void PerformWallChecks()
    {
        foreach (var groundCheck in m_GroundChecks)
        {
            if (Physics.CheckSphere(groundCheck.position, 0.1f, m_GroundLayers, QueryTriggerInteraction.Ignore))
            {
                m_IsGrounded = true;
                break;
            }
        }
    }

    /// <summary>
    /// Execute when the player collides with a trigger volume
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "KillBox")
        {
            TestGameManager.m_Instance.UpdateGameState(GameState.Dead);

            m_IsRunning = false;
            m_IsAlive = false;

            Debug.Log("Killed by " + other.gameObject.tag);
        }
    }

    /// <summary>
    /// Execute when the player collides with an arrow
    /// </summary>
    /// <param name="collision"></param>
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

    /// <summary>
    /// Debug Gizmo drawing
    /// </summary>
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        foreach (var checks in m_GroundChecks)
        {
            Gizmos.DrawWireSphere(checks.transform.position, 0.1f);
        }
        Gizmos.color = Color.green;
        foreach (var checks in m_WallChecks)
        {
            Gizmos.DrawWireSphere(checks.transform.position, 0.1f);
        }

    }

}
