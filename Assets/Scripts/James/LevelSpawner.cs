using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: James Kemeny

public class LevelSpawner : MonoBehaviour
{
    private const float PLAYER_DIST_SPAWN_LEVEL_PART = 100f;

    private PoolManager m_ObjectPooler;
    private GameObject m_Player;

    [Header("Level Parts")]
    [SerializeField] private Transform m_LevelPart_Start;
    [SerializeField] private List<string> m_LevelPartList;

    [Space(10)]
    [Header("Level Settings")]
    private Vector3 m_LastEndPos;

    [Space(10)]
    [Header("Tower Settings")]
    private Vector3 m_NextTowerPos; // Where the next tower will go
    public float m_TowerDistance;
    public float m_TowerSpacing;

    void Start()
    {
        // Get the pool instance and find the player
        m_ObjectPooler = PoolManager.m_Instance;
        m_Player = GameObject.FindGameObjectWithTag("Player");

        m_LastEndPos = m_LevelPart_Start.Find("EndPosition").position;
        SpawnLevelPart();
        int startingSpawnLevelParts = 5;
        for (int i = 0; i < startingSpawnLevelParts; i++)
        {
            SpawnLevelPart();
        }

        m_NextTowerPos = new Vector3(m_TowerSpacing, 0f, m_TowerDistance);
    }

    void Update()
    {
        if(Vector3.Distance(m_Player.transform.position, m_LastEndPos) < PLAYER_DIST_SPAWN_LEVEL_PART)
        {
            SpawnLevelPart();
        }

        // ** Tower Testing **
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnTower();
        }
    }

    private void SpawnLevelPart()
    {
        string chosenLevelPart = m_LevelPartList[Random.Range(0, m_LevelPartList.Count)];

        Transform lastPartPos = SpawnLevelPart(chosenLevelPart, m_LastEndPos);
        m_LastEndPos = lastPartPos.Find("EndPosition").position;
    }

    private Transform SpawnLevelPart(string _tag, Vector3 _spawnPos)
    {
        // Spawn level parts
        GameObject levelPart = m_ObjectPooler.SpawnFromPool(_tag, _spawnPos, Quaternion.identity);
        return levelPart.transform;
    }

    void SpawnTower()
    {
        GameObject tower = m_ObjectPooler.SpawnFromPool("Tower", m_NextTowerPos, Quaternion.identity);
        m_NextTowerPos += new Vector3(m_TowerSpacing, 0.0f, 0.0f);
        //tower.SetActive(true);
    }

}
