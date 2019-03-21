using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBeat : MonoBehaviour
{
    public Rigidbody beatRigidbody;

    private float moveSpeed;

    void Start()
    {
        moveSpeed = -5.0f; 
        //51.283 is the start time, the spawn point is 15 units away, so the beat arrives 3 seconds after it spawns

        beatRigidbody = GetComponent<Rigidbody>();
        Vector3 movement = new Vector3(moveSpeed, 0.0f, 0.0f);
        beatRigidbody.velocity = movement;
        
        
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveSpeed, 0.0f, 0.0f);
        beatRigidbody.velocity = movement;
    }

}
