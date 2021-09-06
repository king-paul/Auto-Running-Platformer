using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: James Kemeny

public class CheckPoint : MonoBehaviour
{
    private TestGameManager m_GM;
    private GameObject m_GFX;
    public float m_RespawnHeight = 5.0f;

    void Start()
    {
        m_GM = TestGameManager.m_Instance;
        m_GFX = gameObject.transform.GetChild(0).gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            m_GM.m_LastCheckpointPos = new Vector3(transform.position.x, transform.position.y + m_RespawnHeight, 0f);
            m_GFX.SetActive(true);
            Debug.Log("Checkpoint set: " + transform.position);
        }
    }

}
