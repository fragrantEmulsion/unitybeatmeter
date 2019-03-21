using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatObject : MonoBehaviour
{
    public int index { get; private set; }

    public void Init(int index)
    {
        this.index = index;
    }
}
