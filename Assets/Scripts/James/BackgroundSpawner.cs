using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    private const float DISTANCE = 200f; // How far away level parts spawn from the player

    private PoolManager m_PoolManager; // Pool manager instance
    private GameObject m_Player; // Player

    [Header("Background Parts")]
    [SerializeField] private Vector3 m_StartPos; // Starting level part
    [SerializeField] private List<string> m_ChunkList = null; // SCUFFED List of chunk names 
    [SerializeField] private Material m_BackGroundMat = null;
    private Vector3 m_LastEndPos;
    public Vector3 m_GroundScale;
    public float m_GroundHeight = -20;
    [Space(10)]
    [Header("Tower Settings")]
    public int m_TowerSpawnRate = 5;


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

        GameObject backgroundChunk = m_PoolManager.SpawnFromPool(chosenLevelPart, m_LastEndPos, Quaternion.identity);

        if (backgroundChunk.transform.Find("EndPosition") != null)
        {
            m_LastEndPos = backgroundChunk.transform.Find("EndPosition").position;
        }

        //Spawn large ground below chunk
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.GetComponentInChildren<MeshRenderer>().material = m_BackGroundMat;
        ground.transform.position = (backgroundChunk.transform.position + m_LastEndPos) / 2;
        ground.transform.position += Vector3.up * m_GroundHeight;
        ground.transform.localScale = m_GroundScale;

        ground.transform.SetParent(backgroundChunk.transform);

        return backgroundChunk.transform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(m_LastEndPos, 5f);
    }
}
