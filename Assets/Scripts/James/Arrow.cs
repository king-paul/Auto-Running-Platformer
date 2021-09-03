using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: James Kemeny

public class Arrow : MonoBehaviour, IPooledObject
{
    
    private Rigidbody m_rb;
    private bool m_HasHit = false;

    public void OnObjectSpawn()
    {   
        m_rb = GetComponent<Rigidbody>();
        m_rb.constraints = RigidbodyConstraints.None;
        
        m_HasHit = false;
    }

    void Update()
    {
        if (!m_HasHit)
        {
            // Rotate the arrow to be facing the rb's velocity 
            transform.rotation = Quaternion.LookRotation(m_rb.velocity.normalized);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.tag != "Arrow")
        {
            // freeze rigidbody constraints and set the new parent
            m_HasHit = true;
            m_rb.constraints = RigidbodyConstraints.FreezeAll;
            //transform.SetParent(collision.transform, false);
        }
    }
}
