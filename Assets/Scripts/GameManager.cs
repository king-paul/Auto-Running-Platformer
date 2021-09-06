using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform m_player;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI coinText;
    public RectTransform jumpMeter;

    [SerializeField]
    private float m_Gravity = -9.8f;
    private bool m_GameRunning;
    private float m_DistanceTravelled;
    private int m_coins;
    private float maxBarHeight;

    private AudioSource m_MusicSource;


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

    // functions / methods
    public void EndGame() { m_GameRunning = false; }
    public void StartGame() { m_GameRunning = true; }

    /// <summary>
    /// Increases the number of coins collected by 1
    /// </summary>
    public void AddCoin() { m_coins++; }

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

    // Start is called before the first frame update
    void Start()
    {
        m_MusicSource = GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>();
        m_GameRunning = true;
        m_coins = 0;

        maxBarHeight = jumpMeter.rect.height;
        jumpMeter.sizeDelta = new Vector2(jumpMeter.rect.width, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameRunning)
        {
            distanceText.text = ((int)m_player.position.x).ToString();
            coinText.text = m_coins.ToString();
        }

        if (!GameRunning && m_MusicSource.isPlaying)
            m_MusicSource.Stop();
    }
}
