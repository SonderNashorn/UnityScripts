using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "new DailySpeech", menuName = "SpeechTree")]
public class SpeechTree : ScriptableObject
{
    public int dayCount;
    public int treeIndex;
    public List<SpeechScriptableObjects> dailyScripts;
}
