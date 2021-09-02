using UnityEngine;

// Author: James Kemeny

public class FollowNode : MonoBehaviour
{
    [Header("Game Objects")]
    private GameObject m_Player;

    [Space(10)]
    [Header("Settings")]
    public float m_SmoothTime = 0.3f;
    private Vector3 m_Velocity = Vector3.zero;
    private Vector3 m_Position;

    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_Position = transform.position;
    }

    void Update()
    {
        Vector3 targetPos = m_Player.transform.TransformPoint(m_Position);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref m_Velocity, m_SmoothTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.5f);
    }
}
