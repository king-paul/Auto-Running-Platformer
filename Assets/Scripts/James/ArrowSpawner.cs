using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: James Kemeny

public class ArrowSpawner : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject m_Target;
    private Transform m_Transform = null;
    private PoolManager m_ObjectPooler;

    [Space(10)]
    [Header("Launcher Settings")]
    public float m_ShotRadius = 5.0f;
    public float m_FireRate = 10.0f;
    public float m_Accuracy = 0.0f;
    private float m_ShotTimer = 0.0f;

    [Space(10)]
    [Header("Projectile Settings")]
    public float m_LaunchSpeed = 20.0f;
    public float m_DespawnTime = 2.0f;

    void Start()
    {
        m_ObjectPooler = PoolManager.m_Instance;
        m_Transform = GetComponent<Transform>();
        if(m_Target == null)
            m_Target = GameObject.FindGameObjectWithTag("Player");

    }
    
    void Update()
    {
        // Store how long since last shot to regulate fire-rate
        m_ShotTimer += Time.deltaTime;
        if (Input.GetMouseButton(0) && m_ShotTimer >= 1 / m_FireRate)
        {
            Shoot();
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
        Gizmos.color = Color.red;
        if (m_Transform != null)
        {
            Gizmos.DrawWireSphere(m_Transform.position, m_ShotRadius);
        }
    }
}
