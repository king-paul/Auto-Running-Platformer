using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: James Kemeny

public class Player : MonoBehaviour
{
    private CharacterController m_Controller;
    private Transform m_Transform = null;
    private LayerMask m_GroundMask;
    private Vector3 m_MoveVelocity;
    private Vector3 m_GroundNormal;

    private Vector3 m_UpVector;

    [Header("Player Settings")]
    //public GameObject m_GroundCheckNode;
    public float m_MoveSpeed = 10.0f;
    public float m_JumpHeight = 5.0f;
    public float m_Gravity = -9.81f;
    public float m_GroundCheckDist = 0.5f;
    public bool m_IsGrounded;
    public bool m_IsRunning;
    public bool m_IsAlive;

    // Start is called before the first frame update
    void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        m_Transform = GetComponentInChildren<Transform>();
        m_GroundMask = LayerMask.GetMask("Ground");
        m_IsAlive = true;
    }



    // Update is called once per frame
    void Update()
    {
        if (m_IsRunning)
        {
            m_IsGrounded = m_Controller.isGrounded;
            if (m_IsGrounded && m_MoveVelocity.y < 0)
            {
                m_MoveVelocity.y = 0f;
            }

            //Vector3 move = Vector3.right;
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            m_Controller.Move(move * Time.deltaTime * m_MoveSpeed);

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }

            // Changes the height position of the player..
            if (Input.GetButtonDown("Jump") && m_IsGrounded)
            {
                m_MoveVelocity.y += Mathf.Sqrt(m_JumpHeight * -3.0f * m_Gravity);
            }

            m_MoveVelocity.y += m_Gravity * Time.deltaTime;
            m_Controller.Move(m_MoveVelocity * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "KillBox")
        {
            Debug.Log("dead");
            m_IsRunning = false;
            m_IsAlive = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            Debug.Log("dead");
            m_IsRunning = false;
            m_IsAlive = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
  
        if (collision.contacts[0].normal.y > 0.7f)
        {//check if ground is walkable, in this case 45 degrees and lower
            m_GroundNormal = collision.contacts[0].normal;//set up direction
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        //Gizmos.DrawWireSphere(m_GroundCheckNode.transform.position, m_GroundCheckDist);
        
    }
}
