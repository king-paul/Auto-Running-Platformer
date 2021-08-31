using UnityEngine;

// Author: James Kemeny

public class KillZone : MonoBehaviour
{
    [Header("Game Objects")]
    private GameObject m_Player;

    [Space(10)]
    [Header("Settings")]
    public float m_SmoothTime = 0.3f;
    private Vector3 m_Velocity = Vector3.zero;
    public float m_PosY;

    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Vector3 targetPos = m_Player.transform.TransformPoint(new Vector3(0, m_PosY, 0));
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref m_Velocity, m_SmoothTime);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }
}
