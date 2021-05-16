using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class EmotionJson
{
    public List<Emotion> datas;
}


[Serializable]
public class Emotion
{
    public string name;
    public float value;
}
