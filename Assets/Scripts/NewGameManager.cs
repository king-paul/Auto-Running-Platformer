using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class NewGameManager : MonoBehaviour
{
    public static NewGameManager m_Instance;
    private AudioSource m_MusicSource;

    [Header("Game Objects")]
    //public Canvas m_Canvas;
    public GameObject m_PlayerText;
    public GameObject m_Arrows;
    [SerializeField] private float m_Gravity = -9.8f;
    private GameObject m_Player;
    private PlayerController playerController;
    public Vector3 m_LastCheckpointPos;

    [Header("GUI")]
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI coinText;
    public RectTransform jumpMeter;
    private float maxBarHeight;

    private int m_coins;
    private bool m_GameRunning;
    private GameState m_State;
   
    public GameState State { get => m_State; }
    public float Gravity { get => m_Gravity; } // Returns the gravity being used in the scene (read only)
    public bool GameRunning { get => m_GameRunning; }  // Returns the current game state to check whether or not the game is

    // functions / methods
    public void StartGame() { UpdateGameState(GameState.Running); }
    public void EndGame() { UpdateGameState(GameState.Dead); }
    
    public void AddCoin() { m_coins++; } // Increases the number of coins collected by 1

    void Awake()
    {
        m_Instance = this;
    }

    void Start()
    {
        // Find gameobjects
        m_Player = GameObject.FindWithTag("Player");
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        m_MusicSource = GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>();
        m_LastCheckpointPos = transform.position;
        // Set ui values
        maxBarHeight = jumpMeter.rect.height;
        jumpMeter.sizeDelta = new Vector2(jumpMeter.rect.width, 0);

        // Set game state
        UpdateGameState(GameState.Idle);
        m_GameRunning = true;
        m_coins = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Play audio
        //if (m_State != GameState.Running && m_MusicSource.isPlaying)
        //    m_MusicSource.Stop();

        // Start running
        if (Input.anyKey && m_State == GameState.Idle)
        {
            Debug.Log("RUN");
            UpdateGameState(GameState.Running);

            m_PlayerText.SetActive(false);
            m_Arrows.GetComponent<ArrowSpawner>().m_Shooting = true;
        }

        // Running
        if (m_State == GameState.Running)
        {
            distanceText.text = ((int)m_Player.transform.position.x).ToString();
            coinText.text = m_coins.ToString();
        }

        if(!m_Player.GetComponent<NewPlayerController>().m_IsAlive)
        {
            m_Player.transform.position = m_LastCheckpointPos;
        }

        // Dead
        if (m_State == GameState.Dead)
        {
            m_Arrows.GetComponentInChildren<ArrowSpawner>().m_Shooting = false;

            m_State = GameState.Idle;

            // restart the scene when a key is pressed
            if (Input.anyKey)
                SceneManager.LoadScene(0);
        }

       
    }

    // Sets the current state of the game. Can be Idls, Running, StageComplete or Dead
    // Takes GameState enumerator as a parameter
    public void UpdateGameState(GameState _newState)
    {
        Debug.Log("New game state: " + _newState);
        m_State = _newState;

        //Animator playerAnim = m_Player.gameObject.GetComponent<Animator>();

        switch (_newState)
        {
            case GameState.Running:
                m_MusicSource.Play();
                playerController.onBegin.Invoke();
                //playerAnim.SetBool("GameRunning", true);
                break;

            case GameState.Dead:
                m_Player.transform.position = m_LastCheckpointPos;
                m_MusicSource.Stop();
                break;

            default:
                //playerAnim.SetBool("GameRunning", false);
                break;
        }


    }
    // Updates the vertical jump meter on the gui to match the player's jump force.
    public void SetJumpMeter(float jumpForce, float maxJumpForce)
    {
        float newHeight = maxBarHeight / maxJumpForce * jumpForce;
        jumpMeter.sizeDelta = new Vector2(jumpMeter.rect.width, newHeight);
    }
}
