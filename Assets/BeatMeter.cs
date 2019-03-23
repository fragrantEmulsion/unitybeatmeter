using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RhythmosEngine;
using UnityEngine;
using UnityEngine.Video;

public class BeatMeter : MonoBehaviour
{
    public Transform transform;
    public RhythmTool rhythmTool;
    public RhythmEventProvider eventProvider;
    public GameObject beatPrefab;
    public AudioClip audioClip;
    public float audioOffset;
    public VideoPlayer videoPlayer;
    public List<BeatPattern> patterns;
    public int bpm;

    private List<BeatObject> beats;
    private ReadOnlyCollection<float> magnitudeSmooth;
    private float songFps;
    private float beatsPerSecond;
    private int frameIndexGap; //space between beats. Currently, 1 section is 4 beats, so each gap should be 

    void Start()
    {
        Application.runInBackground = true;
        
        beats = new List<BeatObject>();
        
        eventProvider.Onset += OnOnset;
        //eventProvider.Beat += OnBeat;
        //eventProvider.Change += OnChange;
        eventProvider.SongLoaded += OnSongLoaded;
        
        magnitudeSmooth = rhythmTool.low.magnitudeSmooth;
        
        rhythmTool.audioClip = audioClip;
        videoPlayer.Prepare();
    }
    private IEnumerator StartWait(float wait)
    {
        yield return new WaitForSeconds(wait);
        rhythmTool.Play ();
    }
    private void OnSongLoaded()
    {   
        songFps = 1 / rhythmTool.frameLength;
        Debug.Log("Song fps: " + songFps);

        beatsPerSecond = bpm / 60.0f; 
        Debug.Log("Beats per second: " + beatsPerSecond);
        
        videoPlayer.Play();
        StartCoroutine(StartWait(audioOffset));
    }

    void Update()
    {
        if (!rhythmTool.songLoaded)        						
            return;
        UpdateBeats();
    }

    private void UpdateBeats()
    {
        //this is the code that removes beats that aren't in the right area, however I prefer to do it via collision
        //collision is better because this is a game engine and I can use the onCollision event to do certain things
        /**
        List<BeatObject> toRemove = new List<BeatObject>();

        foreach (BeatObject beat in beats)
        {
            if (beat.index < rhythmTool.currentFrame || beat.index > rhythmTool.currentFrame + eventProvider.offset)
            {
                Destroy(beat.gameObject);
                
                toRemove.Add(beat);
            }
        }

        foreach (BeatObject beat in toRemove)
        {
            beats.Remove(beat);
        }
        **/
        
        float[] cumMagSmooth = new float[eventProvider.offset + 1];
        float sum = 0;
        for (int i = 0; i < cumMagSmooth.Length; i++)
        {
            int index = Mathf.Min(rhythmTool.currentFrame + i, rhythmTool.totalFrames - 1);

            sum += magnitudeSmooth[index];
            cumMagSmooth[i] = sum;
        }

        foreach (BeatObject beat in beats)
        {
            if (beat != null)
            {
                Vector3 pos = beat.transform.position;
                //Debug.Log(beat.index - rhythmTool.currentFrame);

                pos.x = (beat.index - rhythmTool.currentFrame) * .075f;
                //pos.x -= (magnitudeSmooth[rhythmTool.currentFrame] * .008f * rhythmTool.interpolation);
                //pos.x -= 0.5f;
                beat.transform.position = pos;
            }
        }
    }

    private void OnBeat(Beat beat)
    {
        if (beat.index > 0)
        {
            beats.Add(CreateBeat(beat.index));
        }
        
        Debug.Log("Beat Index: " + beat.index);
    }
    
    //this is where the magic happens
    private void OnOnset(OnsetType type, Onset onset)
    {
        if (type == OnsetType.All)
        {
            BeatPattern pattern = SelectPattern(Random.Range(0, patterns.Count)); //randomly select a supplied pattern object
            List<int> beatList = pattern.pattern; //copy the beat data from the pattern
            
            int indexModifier = 0; //number of frames between spawned beats and the onset which triggers spawning
            int indexIncrement = (bpm / beatList.Count); //fixed value between each beat
            float indexFloatFix = 0.0f; //a value that stores the remainder in case the bpm is not divisible into perfect frames
            
            foreach (int i in beatList)
            {
                //store the remaining part of the float because frames are ints, but bpm fractions are floats
                //if there is a remainder in excess of 1, decrement 1 from storage, and increment the index modifier by 1
                indexFloatFix += Mathf.Repeat(((float)bpm / beatList.Count), 1.0f);
                if (indexFloatFix > 1.0f)
                {
                    indexFloatFix -= 1.0f;
                    indexModifier += 1;
                }
                //spawn the beat and increment the index modifier
                if (i == 1)
                {
                    beats.Add(CreateBeat(onset.index + indexModifier));
                    indexModifier += indexIncrement;
                }
                // still increment the index modifer when there is no beat to be spawned
                else if(i == 0)
                {
                    indexModifier += indexIncrement;
                }
            }
        }
    }


    private BeatObject CreateBeat(int index)
    {
        GameObject beatObject = Instantiate(beatPrefab);
        beatObject.transform.position = this.transform.position;

        BeatObject beat = beatObject.GetComponent<BeatObject>();
        beat.Init(index);

        return beat;
    }

    private void ClearBeats()
    {
        foreach(BeatObject beat in beats)
            Destroy(beat.gameObject);

        beats.Clear();
    }
    
    int FrequencyToSpectrumIndex (float f) {
        var i = Mathf.FloorToInt (f / AudioSettings.outputSampleRate * 2.0f * 1024);
        return Mathf.Clamp (i, 0, 1024);
    }

    private BeatPattern SelectPattern(int index)
    {
        return patterns[index];
    }
    
}
