using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestGameManager : MonoBehaviour
{
    [Header("Game Objects")]
    public Canvas m_Canvas;
    public GameObject m_PlayerText;
    public GameObject m_SpawnNode;
    public GameObject m_Arrows;
    private Player m_Player;

    [Space(10)]
    [Header("Game Settings")]
    public int m_Score = 0;
    //public bool m_GameRunning = false;

    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            m_Player.m_IsRunning = true;
            //m_GameRunning = true;

            m_PlayerText.SetActive(false);
            m_Canvas.gameObject.SetActive(false);
            m_Arrows.GetComponent<ArrowSpawner>().m_Shooting = true;
        }

        if(!m_Player.m_IsAlive)
        {
            m_Arrows.GetComponent<ArrowSpawner>().m_Shooting = false;
            m_Player.transform.position = m_SpawnNode.transform.position;
            m_Player.m_IsAlive = true;
        }

        //while(m_GameRunning)
        //{
        //    
        //}
    }
}
