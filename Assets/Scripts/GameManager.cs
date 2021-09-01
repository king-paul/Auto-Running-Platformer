using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float m_Gravity;
    private bool m_GameRunning;

    // properties
    public bool GameRunning { get => m_GameRunning; }
    public float Gravity { get => m_Gravity; }

    public void EndGame() { m_GameRunning = false; }
    public void StartGame() { m_GameRunning = true; }


    // Start is called before the first frame update
    void Start()
    {
        m_GameRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
