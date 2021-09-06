using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("GUI")]
    
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI coinText;
    public RectTransform jumpMeter;

    [Header("Game Objects")]
    //public Canvas m_Canvas;
    public GameObject m_PlayerText;
    public GameObject m_SpawnNode;
    public GameObject m_Arrows;

    [SerializeField]
    private float m_Gravity = -9.8f;
    private bool m_GameRunning;
    private int m_coins;
    private float maxBarHeight;
    private Transform m_Player;

    private AudioSource m_MusicSource;

    public GameState m_State;
    public static event Action<GameState> OnGameStateChanged;

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

    // functions / methods
    public void EndGame() { m_GameRunning = false; }
    public void StartGame() { m_GameRunning = true; }

    /// <summary>
    /// Increases the number of coins collected by 1
    /// </summary>
    public void AddCoin() { m_coins++; }

    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameObject.FindWithTag("Player").transform;
        m_MusicSource = GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>();
        m_GameRunning = true;
        m_coins = 0;

        maxBarHeight = jumpMeter.rect.height;
        jumpMeter.sizeDelta = new Vector2(jumpMeter.rect.width, 0);

        UpdateGameState(GameState.Idle);
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
            m_Arrows.GetComponent<ArrowSpawner>().m_Shooting = false;
            m_Player.position = m_SpawnNode.transform.position;

            m_State = GameState.Idle;

            // restart the scene
            SceneManager.LoadScene(0);
        }

        if (Input.anyKey && m_State == GameState.Idle)
        {
            UpdateGameState(GameState.Running);

            m_PlayerText.SetActive(false);
            m_Arrows.GetComponent<ArrowSpawner>().m_Shooting = true;
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
        Debug.Log(_newState);
        m_State = _newState;

        Animator playerAnim = m_Player.gameObject.GetComponent<Animator>();

        switch (_newState)
        {
            case GameState.Running: playerAnim.SetBool("GameRunning", true);
                break;
            default: playerAnim.SetBool("GameRunning", false);
                break;
        }
        
        //OnGameStateChanged?.Invoke(_newState);
    }

}
