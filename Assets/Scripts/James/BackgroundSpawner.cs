using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    private const float DISTANCE = 200f; // How far away level parts spawn from the player

    private PoolManager m_PoolManager; // Pool manager instance
    private GameObject m_Player; // Player

    [Header("Level Parts")]
    [SerializeField] private Vector3 m_StartPos; // Starting level part
    [SerializeField] private List<string> m_ChunkList; // SCUFFED List of chunk names 
    [SerializeField] private float m_BackgoundDistance;
    private Vector3 m_LastEndPos;

    [Space(10)]
    [Header("Tower Settings")]
    public int m_TowerSpawnRate = 5;
    private int m_ChunkCount = 0;

    void Start()
    {
        // Get the pool instance and find the player
        m_PoolManager = PoolManager.m_Instance;
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_StartPos = transform.position;
        m_LastEndPos = m_StartPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(m_Player.transform.position, m_LastEndPos) < DISTANCE)
        {
            SpawnBackgroundElement();
            // TODO Spawn a tower after 'spawn rate' number of chunks have been spawned
  
        }
    }

    private Transform SpawnBackgroundElement()
    {
        string chosenLevelPart = m_ChunkList[Random.Range(0, m_ChunkList.Count - 1)];

        GameObject backgroundPart = m_PoolManager.SpawnFromPool(chosenLevelPart, m_LastEndPos, Quaternion.identity);

        if (backgroundPart.transform.Find("EndPosition") != null)
        {
            m_LastEndPos = backgroundPart.transform.Find("EndPosition").position;

        }

        return backgroundPart.transform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(m_LastEndPos, 5f);
    }
}
