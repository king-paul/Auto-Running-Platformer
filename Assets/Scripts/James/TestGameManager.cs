﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Idle,
    Running,
    StageComplete,
    Dead

}

public class TestGameManager : MonoBehaviour
{
    public static TestGameManager m_Instance;
    public GameState m_State;

    public static event Action<GameState> OnGameStateChanged;

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

    private void Awake()
    {
        m_Instance = this;
    }

    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        UpdateGameState(GameState.Idle);
    }


    public void UpdateGameState(GameState _newState)
    {
        Debug.Log(_newState);
        m_State = _newState;

        switch (_newState)
        {
            case GameState.Idle:
                break;
            case GameState.Running:
                break;
            case GameState.StageComplete:
                break;
            case GameState.Dead:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_newState), _newState, null);
        }
        OnGameStateChanged?.Invoke(_newState);
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            UpdateGameState(GameState.Running);

            //m_Player.m_IsRunning = true;
            //m_GameRunning = true;

            m_PlayerText.SetActive(false);
            //m_Canvas.gameObject.SetActive(false);
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