using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: James Kemeny

public class LevelSpawner : MonoBehaviour
{
    private PoolManager m_ObjectPooler;
    private GameObject m_Player;

    [Header("Level Settings")]
    private Vector3 m_StartPos; // Where the ground tiles will start
    private Vector3 m_NextPos; // Where the next tile will go
    public float m_XOffset;
    public float m_YOffset;
    public float m_SpawnDistance;
    public bool m_CanSpawnGround;

    [Space(10)]
    [Header("Tower Settings")]
    private Vector3 m_TowerStartPos; // Where the ground tiles will start
    private Vector3 m_NextTowerPos; // Where the next tower will go
    public float m_TowerDistance;
    public float m_TowerSpacing;

    void Start()
    { 
        // Get the pool instance and find the player
        m_ObjectPooler = PoolManager.m_Instance;
        m_Player = GameObject.FindGameObjectWithTag("Player");

        // Set start pos to this gamobject's pos
        m_StartPos = gameObject.transform.position;
        // Initialize next to start
        m_NextPos = m_StartPos;

        // Do the same for towers
        m_TowerStartPos = m_StartPos + new Vector3(0f, 0f, m_TowerDistance);
        m_NextTowerPos = m_TowerStartPos;

        m_CanSpawnGround = true;
    }

    void Update()
    {
        //Initialize start ground segment
        if (m_CanSpawnGround)
        {
            SpawnLevelSegment();
        }

        // ** Testing **
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnTower();
        }

        if (Vector3.Distance(m_NextPos, m_Player.transform.position) < m_SpawnDistance)
        {
            SpawnLevelSegment();
        }
    }

    void SpawnLevelSegment()
    {
        GameObject ground = m_ObjectPooler.SpawnFromPool("Ground", m_NextPos, Quaternion.identity);
        m_NextPos += new Vector3(m_XOffset, Random.Range(-m_YOffset/2, m_YOffset/2), 0.0f);
        ground.SetActive(true);

        m_CanSpawnGround = false;
    }

    void SpawnTower()
    {
        GameObject tower = m_ObjectPooler.SpawnFromPool("Tower", m_NextTowerPos, Quaternion.identity);
        m_NextTowerPos += new Vector3(m_TowerSpacing, 0.0f, 0.0f);
        tower.SetActive(true);
    }
}
