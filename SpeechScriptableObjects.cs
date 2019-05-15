using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "new Speech", menuName ="Speech")]
public class SpeechScriptableObjects : ScriptableObject
{
    public int offerToSell;
    public string speakerName;
    public List<TextManager.SpeechGroup> speechGroup;
    public int drugID;
    public Texture2D drugSprite;
}
