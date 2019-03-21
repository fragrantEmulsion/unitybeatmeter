using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBeat : MonoBehaviour
{
    public GameObject beat;
    public double startWait; //time before beats start 
    public double BPM;
    public AudioSource music;
    
    private bool canSpawn;
    private double nextSpawnTime;
    private double spawnGap;
    private double breakGap;

    public List<BeatPattern> patterns;

    // Start is called before the first frame update
    void Start()
    {
        canSpawn = true;
        nextSpawnTime = music.time + startWait;
        spawnGap = (60.0 / BPM);
        breakGap = spawnGap;
    }

    void Update()
    {
        if (canSpawn && music.time > nextSpawnTime)
        {
            nextSpawnTime = music.time + spawnGap;
            Instantiate(beat, new Vector3(15f, -3.5f, -15f), Quaternion.identity); //hardcode spawn position
        }
    }
}
