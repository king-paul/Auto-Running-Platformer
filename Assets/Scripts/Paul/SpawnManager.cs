using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    private Vector3 spawnPos = new Vector3(25, 0, 0);
    private GameManager gameManager;

    private float startDelay = 3f;
    private float repeatRate = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Player").GetComponent<GameManager>();
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnObstacle()
    {
        if (gameManager.GameRunning)
        {
            int randomIndex = Random.Range(0, obstaclePrefabs.Length);
            Instantiate(obstaclePrefabs[randomIndex], spawnPos, obstaclePrefabs[randomIndex].transform.rotation);
        }
    }

}
