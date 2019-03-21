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

    private void OnOnset(OnsetType type, Onset onset)
    {
        if (type == OnsetType.All)
        {
            BeatPattern pattern = SelectPattern(Random.Range(0, patterns.Count));
            List<int> beatList = pattern.pattern;
            int indexIncrement = 0;
            foreach (int i in beatList)
            {
            
                if (i == 1)
                {
                    beats.Add(CreateBeat(onset.index + indexIncrement));
                    indexIncrement += 15;
                }
                else if(i == 0)
                {
                    indexIncrement += 15; //this should be variable based on bpm, figure out soon
                }
                Debug.Log(i);
            }

            indexIncrement = 0;
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
