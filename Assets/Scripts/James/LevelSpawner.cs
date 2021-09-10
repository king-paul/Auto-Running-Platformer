using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: James Kemeny

public class LevelSpawner : MonoBehaviour
{

    private const float PLAYER_DIST_SPAWN_LEVEL_CHUNK = 100f; // How far away level parts spawn from the player

    private PoolManager m_ObjectPooler; // Pool manager instance
    private GameObject m_Player; // Player

    //
    // Level Spawner Requirements:
    // - Spawned chunks need an end position node with the name EndPosition
    // - This is updated each time one is spawned, so that every chunk is sure to follow on from the last.
    //

    [Header("Level Parts")]
    [SerializeField] private Transform m_Start_Chunk; // Starting level part
    [SerializeField] private List<string> m_ChunkList; // SCUFFED List of chunk names 
    private Vector3 m_LastEndPos;

    [Space(10)]
    [Header("Level Settings")]

    [Space(10)]
    [Header("Tower Settings")]
    private Vector3 m_NextTowerPos; // Where the next tower will go
    public float m_TowerDistance;
    public float m_TowerSpacing;
    public int m_TowerSpawnRate = 5;
    private int m_ChunkCount = 0;

    void Start()
    {
        // Get the pool instance and find the player
        m_ObjectPooler = PoolManager.m_Instance;
        m_Player = GameObject.FindGameObjectWithTag("Player");

        // If theres no starting chunk set, setup a new one
        if (!m_Start_Chunk)
        {
            m_LastEndPos = SpawnChunk().Find("EndPosition").position;

        }
        else
        {
            // Set the last end pos to the start chunk's end pos
            m_LastEndPos = m_Start_Chunk.Find("EndPosition").position;
        }

        m_NextTowerPos = new Vector3(m_TowerSpacing, 0f, m_TowerDistance);
    }

    void Update()
    {
        // Check the players distance for object spawning
        if(Vector3.Distance(m_Player.transform.position, m_LastEndPos) < PLAYER_DIST_SPAWN_LEVEL_CHUNK)
        {
            SpawnChunk();
            // Spawn a tower after 'spawn rate' number of chunks have been spawned
            if(m_ChunkCount == m_TowerSpawnRate)
            {
                SpawnTower();
                m_ChunkCount = 0;
               
            }
            else
            {
                m_ChunkCount++;
            }
        }
    }

    /// <summary>
    /// Randomly spawns a chunk object from a pool using the names of the objects you'd like to spawn
    /// Chunks must have an end position node, and lastLevelPos stores the last one and is updated each time one is spawned
    /// </summary>
    /// <returns></returns>
    private Transform SpawnChunk()
    {
        string chosenLevelPart = m_ChunkList[Random.Range(0, m_ChunkList.Count)];

        GameObject levelPart = m_ObjectPooler.SpawnFromPool(chosenLevelPart, m_LastEndPos, Quaternion.identity);
        m_LastEndPos = levelPart.transform.Find("EndPosition").position;
        return levelPart.transform;
    }

    void SpawnTower()
    {
        GameObject tower = m_ObjectPooler.SpawnFromPool("Tower", m_NextTowerPos, Quaternion.identity);
        m_NextTowerPos += new Vector3(m_TowerSpacing, 0.0f, 0.0f);
    }

}
