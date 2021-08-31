using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    private Vector3 startPos;
    private float repeatWidth;

    // Start is called before the first frame update
    void Start()
    {
        // assign startPos to be the starting position of the object
        startPos = transform.position;
        repeatWidth = GetComponent<BoxCollider>().size.x / 2; // gets half the width of the background
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < startPos.x - repeatWidth)
        {
            transform.position = startPos;
        }

        
    }
}
