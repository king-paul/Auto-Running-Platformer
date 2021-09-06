using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: James Kemeny

public class TrailRandomizer : MonoBehaviour
{
    private TrailRenderer m_TrailRenderer;
    private float m_Duration;
    private float m_TimeStamp;

    void Start()
    {
        m_TrailRenderer = GetComponent<TrailRenderer>();
    }

    
    void Update()
    {
        if(Time.time > m_TimeStamp + m_Duration)
        {
            m_Duration = Random.Range(0.05f, 0.3f);
            m_TimeStamp = Time.time;
            m_TrailRenderer.emitting = !m_TrailRenderer.emitting;
        }
    }
}
