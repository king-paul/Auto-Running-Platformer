using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform target;
    float yOffset = 5f;
    float zOffset = -10f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(target.position.x, 
            target.position.y + yOffset, target.position.z + zOffset);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.position.x, 
            target.position.y + yOffset, target.position.z + zOffset);
    }
}
