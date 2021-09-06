using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private TestGameManager m_GM;
    private GameObject m_GFX;
    void Start()
    {
        m_GM = TestGameManager.m_Instance;
        m_GFX = gameObject.transform.GetChild(0).gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            m_GM.m_LastCheckpointPos = new Vector3(transform.position.x, transform.position.y, 0f);
            m_GFX.SetActive(true);
            Debug.Log("Checkpoint set: " + transform.position);
        }
    }

}
