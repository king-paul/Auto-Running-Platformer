using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerOld : MonoBehaviour
{
    [Header("GUI")]

    public GameObject m_HUD;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI gameOverDistanceText;
    public TextMeshProUGUI coinText;
    public RectTransform jumpMeter;
    public GameObject m_TitleText;
    public GameObject m_GameOverUI;

    [Header("Game Objects")]    
    public GameObject m_SpawnNode;
    public GameObject m_Arrows;

    [SerializeField]
    private float m_Gravity = -9.8f;
    private bool m_GameRunning;
    private int m_coins;
    private float maxBarHeight;
    private Transform m_Player;
    private PlayerController playerController;
    Vector3 m_LastCheckpointPos;

    private AudioSource m_MusicSource;
    private GameState m_State;

    // properties
    /// <summary>
    /// Returns the current game state to check whether or not the game is
    /// still running (read only)
    /// </summary>
    public bool GameRunning { get => m_GameRunning; }
    /// <summary>
    /// Returns the gravity being used in the scene (read only)
    /// </summary>
    public float Gravity { get => m_Gravity; }
    public GameState State { get => m_State; }
    public Vector3 LastCheckpointPos { 
        get => m_LastCheckpointPos; 
        set => LastCheckpointPos = value; 
    }

    // functions / methods
    public void EndGame() { UpdateGameState(GameState.Dead); }
    public void StartGame() { UpdateGameState(GameState.Running); }

    /// <summary>
    /// Increases the number of coins collected by 1
    /// </summary>
    public void AddCoin() { m_coins++; }

    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameObject.FindWithTag("Player").transform;
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        m_MusicSource = GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>();
        m_GameOverUI.SetActive(false);

        m_LastCheckpointPos = m_SpawnNode.transform.position;
        m_GameRunning = true;
        m_coins = 0;

        maxBarHeight = jumpMeter.rect.height;
        jumpMeter.sizeDelta = new Vector2(jumpMeter.rect.width, 0);

        UpdateGameState(GameState.Idle);

        m_Player.position = m_LastCheckpointPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_State != GameState.Running && m_MusicSource.isPlaying)
            m_MusicSource.Stop();

        if (m_State == GameState.Running)
        {
            distanceText.text = ((int)m_Player.position.x).ToString();
            coinText.text = m_coins.ToString();
        }

        if (m_State == GameState.Dead)
        {
            //m_Arrows.GetComponent<ArrowSpawner>().m_Shooting = false;
            //m_Player.position = m_SpawnNode.transform.position;

            //m_State = GameState.Idle;

            // restart the scene when a key is pressed
            //if(Input.anyKey)
                //SceneManager.LoadScene(0);
                
        }

        if (m_State == GameState.Idle && Input.anyKey)
        {
            UpdateGameState(GameState.Running);
            m_TitleText.SetActive(false);
           
        }

    }

    /// <summary>
    /// Updates the vertical jump meter on the gui to match the player's jump force.
    /// </summary>
    /// <param name="jumpForce">Sets the current value to set the bar at</param>
    /// <param name="maxJumpForce">Sets tge maximum value that the jump meter can be set at</param>
    public void SetJumpMeter(float jumpForce, float maxJumpForce)
    {
        float newHeight = maxBarHeight / maxJumpForce * jumpForce;
        jumpMeter.sizeDelta = new Vector2(jumpMeter.rect.width, newHeight);
    }

    /// <summary>
    /// Sets the current state of the game. Can be Idls, Running, StageComplete or Dead
    /// </summary>
    /// <param name="_newState">Takes GameState enumerator as a parameter</param>
    public void UpdateGameState(GameState _newState)
    {
        //Debug.Log(_newState);
        m_State = _newState;

        switch (_newState)
        {
            case GameState.Running:
                m_GameOverUI.SetActive(false);
                m_HUD.SetActive(true);
                m_Player.position = m_LastCheckpointPos;
                //m_Arrows.GetComponent<ArrowSpawner>().m_Shooting = true;
                m_MusicSource.Play();
                playerController.onBegin.Invoke();
                break;

            case GameState.Dead: 
                m_MusicSource.Stop();
                m_HUD.SetActive(false);
                m_GameOverUI.SetActive(true);
                gameOverDistanceText.text = distanceText.text;
                break;

            default: 
                break;
        }        
        
    }

}
