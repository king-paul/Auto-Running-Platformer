using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: James Kemeny

public class CameraController : MonoBehaviour
{
    private GameObject m_Player;

    [Header("Camera Movement")]
    public float m_SmoothTime = 0.3f;
    public Vector3 m_CamOffset;
    private Vector3 m_Velocity = Vector3.zero;
    public float m_PosY;

    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_CamOffset = transform.position;
    }

    void Update()
    {
        
        Vector3 targetPos = m_Player.transform.position + m_CamOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref m_Velocity, m_SmoothTime);
    }
}
