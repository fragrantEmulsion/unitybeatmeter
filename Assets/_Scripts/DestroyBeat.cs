using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBeat : MonoBehaviour
{

    public GameObject particles;
    //public MeshRenderer mesh;
    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        Instantiate(particles, new Vector3(
            transform.position.x, 
            transform.position.y, 
            transform.position.z), 
            Quaternion.identity);
    }
}