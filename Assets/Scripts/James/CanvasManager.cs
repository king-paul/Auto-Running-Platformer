using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: James Kemeny

public class CanvasManager : MonoBehaviour
{
    public GameObject m_TitleText;
    private GameManager m_GameManager;

    private void Start()
    {
        m_GameManager = GameManager.m_Instance;
    }

    void Update()
    {
        if(m_GameManager.State == GameState.Idle)
        {
            m_TitleText.gameObject.SetActive(true);
        }
        if (m_GameManager.State == GameState.Running)
        {
            m_TitleText.gameObject.SetActive(false);
        }
    }
}
