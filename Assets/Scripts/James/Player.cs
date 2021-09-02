using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: James Kemeny

public class Player : MonoBehaviour
{
    private CharacterController m_Controller;
    private Transform m_Transform = null;
    private Vector3 m_MoveVelocity;

    [Header("Player Settings")]
    //public GameObject m_GroundCheckNode;
    public float m_MoveSpeed = 10.0f;
    public float m_Gravity = -9.81f;
    public float m_GroundCheckDist = 0.5f;
    public float m_JumpVelocity = 5.0f;
    public float m_FallMultiplier = 2.5f;
    public float m_LowJumpMultiplier = 2f;
    
    public bool m_IsGrounded;
    public bool m_IsRunning;
    public bool m_IsAlive;

    private float m_GroundedTimer;

    public void OnAwake()
    {
        TestGameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    public void OnDestroy()
    {
        TestGameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState _state)
    {
        m_IsRunning = (_state == GameState.Running);

        Debug.Log(_state);

    }

    public void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        m_Transform = GetComponentInChildren<Transform>();
        m_IsAlive = true;
    }

    public void Update()
    {
        m_IsRunning = (TestGameManager.m_Instance.m_State == GameState.Running);

        if (m_IsRunning)
        {
            m_IsGrounded = m_Controller.isGrounded;

            if (m_IsGrounded)
            {
                m_GroundedTimer = 0.2f;
            }
            if (m_GroundedTimer > 0)
            {
                m_GroundedTimer -= Time.deltaTime;
            }

            if (m_IsGrounded && m_MoveVelocity.y < 0)
            {
                m_MoveVelocity.y = 0f;
            }

            m_MoveVelocity.y += m_Gravity * Time.deltaTime;

            //Vector3 move = Vector3.right;
            // X and Y controls for testing
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            move *= m_MoveSpeed;

            if (Input.GetButtonDown("Jump"))
            {
                if(m_GroundedTimer > 0)
                {
                    m_GroundedTimer = 0;
                    m_MoveVelocity += Vector3.up * m_JumpVelocity;

                }
            }
            // Jump handling
            if (m_Controller.velocity.y < 0)
            {
                // If falling
                m_MoveVelocity += (Vector3.up * m_Gravity * (m_FallMultiplier - 1) * Time.deltaTime);
                Debug.Log("Falling");
            }
            else if (m_Controller.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                //Low jump multiplier
                m_MoveVelocity += (Vector3.up * m_Gravity * (m_LowJumpMultiplier - 1) * Time.deltaTime);
                Debug.Log("LowJump");
            }

            m_Controller.Move(move * Time.deltaTime);

            //if (move != Vector3.zero)
            //{
            //    gameObject.transform.forward = move;
            //}
            //
            //// Changes the height position of the player..
            //if (Input.GetButtonDown("Jump") && m_IsGrounded)
            //{
            //    m_MoveVelocity.y += Mathf.Sqrt(m_JumpHeight * -3.0f * m_Gravity);
            //}

            

            m_MoveVelocity.y += m_Gravity * Time.deltaTime;
            m_Controller.Move(m_MoveVelocity * Time.deltaTime);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        //Gizmos.DrawWireSphere(m_GroundCheckNode.transform.position, m_GroundCheckDist);
        
    }
}
