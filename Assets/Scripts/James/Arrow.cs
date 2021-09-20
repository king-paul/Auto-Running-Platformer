﻿using System.Collections;
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

    public void OnObjectSpawn()
    {
        m_GameManager = GameManager.m_Instance;
        m_rb = GetComponent<Rigidbody>();
        m_rb.constraints = RigidbodyConstraints.None;
        GetComponent<BoxCollider>().enabled = true;
        m_HasHit = false;
    }

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

    private void OnCollisionEnter(Collision collision)
    {
        timer = m_DespawnTime;

        if (collision.collider.tag != "Arrow")
        {
            m_Spawner.OnArrowHit();
            // freeze rigidbody constraints and set the new parent
            m_HasHit = true;
            m_rb.constraints = RigidbodyConstraints.FreezeAll;
            this.GetComponent<BoxCollider>().enabled = false;
            //transform.SetParent(collision.transform, false);
        }
    }
}
