using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: James Kemeny

public class Arrow : MonoBehaviour, IPooledObject
{
    private GameManager m_GameManager;
    private ArrowSpawner m_Spawner;
    private Rigidbody m_rb;
    private bool m_HasHit = false;
    public float m_DespawnTime = 2.0f;
    private float timer = 0f;

    /// <summary>
    /// Once spawned, get the game manager, 
    /// Find the arrow spawner object in the scene
    /// </summary>
    public void Start()
    {
        m_GameManager = GameManager.m_Instance;
        m_Spawner = GameObject.FindGameObjectWithTag("ArrowSpawner").GetComponent<ArrowSpawner>();
        
    }

    /// <summary>
    /// Get the rigidbody component and set constraints and 
    /// enable the box collider
    /// </summary>
    public void OnObjectSpawn()
    {
        
        m_rb = GetComponent<Rigidbody>();
        m_rb.constraints = RigidbodyConstraints.None;
        GetComponent<BoxCollider>().enabled = true;
        m_HasHit = false;
    }

    /// <summary>
    /// If the arrow hasn't collided with anything, rotate it to the rb's velocity direction
    /// otherwise, start the despawn timer, which diables the object when it expires
    /// </summary>
    void Update()
    {
        if (!m_HasHit)
        {
            // Rotate the arrow to be facing the rb's velocity 
            transform.rotation = Quaternion.LookRotation(m_rb.velocity.normalized);
        }
        else
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        if (m_GameManager.State == GameState.Idle)
        {
            timer = 0;
        }
    }

    /// <summary>
    /// Checks other obj tag and sets collider and rigidbody according to whether it has hit
    /// </summary>
    /// <param name="collision"> collision data from collision event </param>
    private void OnCollisionEnter(Collision collision)
    {
        timer = m_DespawnTime;

        if (collision.collider.tag != "Arrow")
        {
            if(m_Spawner)
            {
                m_Spawner.OnArrowHit();
                //m_Spawner.m_HitTransform.position = transform.position;
            }
            // freeze rigidbody constraints and set the new parent
            m_HasHit = true;
            m_rb.constraints = RigidbodyConstraints.FreezeAll;
            this.GetComponent<BoxCollider>().enabled = false;
            //transform.SetParent(collision.transform, false);
        }
    }
}
