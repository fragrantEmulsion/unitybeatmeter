using System.Collections;
using System.Collections.Generic;
using RhythmosEngine;
using UnityEngine;

public class RhythmPlayer : MonoBehaviour
{
    private RhythmosDatabase m_rhythmosDatabase;
    public TextAsset m_rhythmosFile;

    private static RhythmPlayer m_instance;

    public static RhythmPlayer g_instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = (RhythmPlayer) GameObject.FindObjectOfType(typeof(RhythmPlayer));
                
                DontDestroyOnLoad(m_instance.gameObject);
            }

            return m_instance;
        }
    }

    void Start()
    {
        if (m_instance == null)
        {
            m_instance = this;

            m_rhythmosDatabase = new RhythmosDatabase();

            m_rhythmosDatabase.LoadRhythmosDatabase(m_rhythmosFile);

            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != m_instance)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void Update()
    {
        //Debug.Log(m_rhythmosDatabase.RhythmsCount.ToString());
    }
}
