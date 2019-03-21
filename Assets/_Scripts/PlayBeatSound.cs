using UnityEngine;
using System.Collections;

public class PlayBeatSound : MonoBehaviour
{
    public AudioClip beatSound;

    void Start ()   
    {
        GetComponent<AudioSource>().playOnAwake = false;
        GetComponent<AudioSource>().clip = beatSound;
    }        
 
    void OnTriggerEnter () 
    {
        GetComponent<AudioSource>().Play ();
    }
}