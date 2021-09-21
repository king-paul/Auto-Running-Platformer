using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public enum GameState
{
    Idle,
    Running,
    StageComplete,
    Dead
}

public class GameManager : MonoBehaviour
{
    public static GameManager m_Instance;
    private AudioSource m_MusicSource;

    [Header("Game Objects")]
    //public Canvas m_Canvas;
    public GameObject m_Arrows;
    public Transform m_StartingPoint;

    [Header("Variables")]
    [SerializeField] private float m_Gravity = -9.8f;
    private GameObject m_Player;
    private PlayerController playerController;
    public Vector3 m_LastCheckpointPos;

    [Header("Music")]
    public AudioClip m_TitleMusic;
    public AudioClip m_RunningMusic;

    private int m_coins;
    private bool m_GameRunning;
    private GameState m_State;

    private GuiController gui;

    const int CONTINUE_COST = 5;

    // properties
    public GameState State { get => m_State; }
    /// <summary>
    /// Returns the gravity being used in the scene (read only)
    /// </summary>
    public float Gravity { get => m_Gravity; }

    /// <summary>
    /// Returns the current game state to check whether or not the game is
    /// still running (read only)
    /// </summary>
    public bool GameRunning { get => m_GameRunning; }

    /// <summary>
    /// Used to access the last checkpoint position variable
    /// </summary>
    public Vector3 LastCheckpointPos
    {
        get => m_LastCheckpointPos;
        set => LastCheckpointPos = value;
    }
    

    // functions / methods
    public void StartGame() { UpdateGameState(GameState.Running); }
    public void EndGame() { UpdateGameState(GameState.Dead); }
    public void RestartGame()
    {
        // set the last checkpoint to the start of the level
        m_LastCheckpointPos = new Vector3(m_StartingPoint.position.x,
            m_StartingPoint.position.y, 0);

        UpdateGameState(GameState.Idle);
    }

    public void RestartScene() { SceneManager.LoadScene(0); }
    public void QuitGame() {
        Debug.Log("Quit Button Clicked");
        Application.Quit(); }

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
        gui = GameObject.FindWithTag("Canvas").GetComponent<GuiController>();

        m_LastCheckpointPos = transform.position;

        // Set ui values
        gui.SetJumpMeter(0, 0);

        // Set game state
        UpdateGameState(GameState.Idle);
        m_GameRunning = true;
        m_coins = 0;

        // Set music to title music
        m_MusicSource.clip = m_TitleMusic;
        m_MusicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // Running
        if (m_State == GameState.Running)
        {
            gui.distanceText.text = ((int)m_Player.transform.position.x).ToString();
            gui.coinText.text = m_coins.ToString();
        }

        // Dead
        /*if (m_State == GameState.Dead && m_Arrows != null)
        {
            m_Arrows.GetComponentInChildren<ArrowSpawner>().m_Shooting = false;
            m_State = GameState.Idle;
        }*/

        // Quit the game when ESC is pressed
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("You chose to quit the application");
            Application.Quit();
        }
       
    }

    // Sets the current state of the game. Can be Idls, Running, StageComplete or Dead
    // Takes GameState enumerator as a parameter
    public void UpdateGameState(GameState _newState)
    {
        //Debug.Log("New game state: " + _newState);
        m_State = _newState;

        switch (_newState)
        {
            case GameState.Idle:                
                gui.titleScreen.SetActive(true);
                m_Player.transform.position = m_LastCheckpointPos;
                gui.gameOverUI.SetActive(false);                
            break;

            case GameState.Running:
                m_Player.transform.position = m_LastCheckpointPos;
                m_Player.GetComponent<CharacterController>().enabled = true;
                m_coins = Globals.coins;

                gui.titleScreen.SetActive(false);

                if(m_Arrows != null)
                    m_Arrows.GetComponent<ArrowSpawner>().CeaseFire();

                gui.titleScreen.SetActive(false);
                gui.gameOverUI.SetActive(false);
                gui.HUD.SetActive(true);

                m_MusicSource.clip = m_RunningMusic;
                m_MusicSource.Play();

                playerController.onBegin.Invoke();
            break;

            case GameState.Dead:
                Debug.Log("Last Checkpoint" + m_LastCheckpointPos);

                //m_MusicSource.Stop();

                gui.HUD.SetActive(false);
                gui.ShowGameOverScreen(m_coins, CONTINUE_COST);

                gui.gameOverDistanceText.text = (int)m_Player.transform.position.x + " feet";

                Globals.coins = m_coins;
                //PlayerPrefs.SetInt("coins", m_coins);
            break;

            default:                
                break;
        }

    }
    // Updates the vertical jump meter on the gui to match the player's jump force.
    public void SetJumpMeter(float jumpForce, float maxJumpForce)
    {
        gui.SetJumpMeter(jumpForce, maxJumpForce);
    }

    public void ContinueFromCheckpoint()
    {
        if(m_coins >= CONTINUE_COST)
        {
            m_coins -= CONTINUE_COST;
            Globals.coins = m_coins;
            UpdateGameState(GameState.Running);
        }
    }

}
