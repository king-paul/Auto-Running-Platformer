using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: James Kemeny

public class ArrowSpawner : MonoBehaviour
{
    private NewGameManager m_GameManager;

    [Header("Game Objects")]
    public GameObject m_Target;
    private Transform m_Transform = null;
    private PoolManager m_ObjectPooler;

    [Space(10)]
    [Header("Launcher Settings")]
    public float m_ShotRadius = 5.0f;
    public float m_FireRate = 10.0f;
    private float m_ShotTimer = 0.0f;
    public bool m_Shooting = false;

    [Space(10)]
    [Header("Projectile Settings")]
    public float m_LaunchSpeed = 20.0f;

    void Start()
    {
        m_GameManager = NewGameManager.m_Instance;
        m_ObjectPooler = PoolManager.m_Instance;
        m_Transform = GetComponent<Transform>();
        if(m_Target == null)
            m_Target = GameObject.FindGameObjectWithTag("Player");

    }
    
    void Update()
    {
        m_Shooting = (m_GameManager.State == GameState.Running);

        // Store how long since last shot to regulate fire-rate
        m_ShotTimer += Time.deltaTime;
        if (m_Shooting && m_ShotTimer >= 1 / m_FireRate)
        {
            Shoot();
        }
        if (m_GameManager.State == GameState.Dead)
        {
            m_Shooting = false;
        }
    }

    void Shoot()
    {
        // Store a new start position for each arrow within a sphere at the emitter's position
        Vector3 shotPos = (Random.insideUnitSphere * m_ShotRadius) + m_Transform.position;
        // Store a normalized direction vector to the target
        Vector3 direction = m_Target.transform.position - shotPos;
        direction.Normalize();

        // Spawn object from pool at the starting pos
        GameObject pooledObj = m_ObjectPooler.SpawnFromPool("Arrow", shotPos, Quaternion.identity);
        // Get the object rigidbody and apply a force in the target direction
        Rigidbody rb = pooledObj.GetComponent<Rigidbody>();
        rb.velocity = direction * m_LaunchSpeed;

        // Reset shot timer 
        m_ShotTimer = 0.0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, Vector3.one);

        Gizmos.color = Color.red;
        if (m_Transform != null)
        {
            Gizmos.DrawWireSphere(m_Transform.position, m_ShotRadius);
        }
    }
}
