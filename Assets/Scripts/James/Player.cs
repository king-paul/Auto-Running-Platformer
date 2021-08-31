using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: James Kemeny

public class Player : MonoBehaviour
{
    private CharacterController m_Controller;
    private Transform m_Transform = null;
    private Vector3 m_MoveVelocity;
    public float m_MoveSpeed = 10.0f;
    public float m_Gravity = -9.81f;
    public float m_JumpHeight = 5.0f;
    private bool m_IsGrounded;

    // Start is called before the first frame update
    void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        m_Transform = GetComponentInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        m_IsGrounded = m_Controller.isGrounded;
        
        if (m_IsGrounded && m_MoveVelocity.y < 0f)
        {
            m_MoveVelocity.y = 0f;
        }

        Vector3 direction = Vector3.right;
        m_Controller.Move((direction * m_MoveSpeed) * Time.deltaTime);
        
        //if (direction != Vector3.zero)
        //{
        //    m_Transform.forward = direction;
        //}

        if (Input.GetButtonDown("Jump") && m_IsGrounded)
        {
            Debug.Log("jump");
            m_MoveVelocity.y += Mathf.Sqrt(m_JumpHeight * -3.0f * m_Gravity);
        }

        m_MoveVelocity.y += m_Gravity * Time.deltaTime;
        m_Controller.Move(m_MoveVelocity * Time.deltaTime);
    }

}
