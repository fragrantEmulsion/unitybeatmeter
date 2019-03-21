using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Beats/Objects/BeatPattern", order = 1)]
public class BeatPattern : ScriptableObject
{
    public string name;
    public int difficulty;
    public List<int> pattern;
}
