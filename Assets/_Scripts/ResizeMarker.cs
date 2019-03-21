using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeMarker : MonoBehaviour
{
    public Transform transform;

    public RhythmTool rhythmTool;

    public RhythmEventProvider eventProvider;

    public AudioClip audioClip;

    private int count;
    private bool ready;

    void Start()
    {
        eventProvider.SongLoaded += OnSongLoaded;
        eventProvider.Beat += OnBeat;
        eventProvider.Change += OnChange;
        
        rhythmTool.audioClip = audioClip;
        count = 0;
        ready = false;
    }

    private void OnSongLoaded()
    {
        rhythmTool.Play();
    }
    
    private void OnChange(int index, float change)
    {
        if (change > 0)
        {
            ready = true;
            Debug.Log("change");
        } 
    }
    private void OnBeat(Beat beat)
    {
        if (ready)
        {
            count++;
            transform.localScale = new Vector3(0.01f * count, 0.01f * count, 0.01f * count);
        }
    }
}
