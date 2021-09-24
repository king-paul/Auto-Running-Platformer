using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: James Kemeny

public class CheckPoint : MonoBehaviour
{
    private GameManager m_GameManager;
    private GameObject m_GFX;
    public float m_RespawnHeight = 5.0f;

    void Start()
    {
        m_GameManager = GameManager.m_Instance;
        m_GFX = gameObject.transform.GetChild(0).gameObject;
    }

    /// <summary>
    /// If other is a player, set the last checkpoint position and set the fire gfx to active
    /// </summary>
    /// <param name="other"> Collider of the other object </param>
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            m_GameManager.m_LastCheckpointPos = new Vector3(transform.position.x, transform.position.y + m_RespawnHeight, 0f);
            m_GFX.SetActive(true);
            Debug.Log("Checkpoint set: " + transform.position);
        }
    }

}
