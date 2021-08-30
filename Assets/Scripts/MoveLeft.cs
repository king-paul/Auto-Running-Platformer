using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 20f;
    private float leftBound = -15;
    private PlayerController playerControllerScript;

    enum State { standing, walking, waiting, running };

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerControllerScript.GameOver == false && playerControllerScript.gameState == PlayerController.State.running)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }        

        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }

    }
}
