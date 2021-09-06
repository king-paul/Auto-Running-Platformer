using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOL : MonoBehaviour
{
    TestGameManager m_GameManager;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Debug.Log("DDOL " + gameObject.name);

        m_GameManager = Object.FindObjectOfType<TestGameManager>();
    }

}
