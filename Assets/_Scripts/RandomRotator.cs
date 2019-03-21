using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour {
    public float tumblr;
    public Rigidbody objectRigidbody;
    public float x, y, z;
    void Start() {
        objectRigidbody = GetComponent<Rigidbody>();
        objectRigidbody.angularVelocity = new Vector3(x, y, z) * tumblr;
    }
}
