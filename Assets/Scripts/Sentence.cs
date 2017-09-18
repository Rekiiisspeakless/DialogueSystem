using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Sentence{
    public enum TypeOfSentence{
        normalSentence, selection
    };
    public TypeOfSentence sentenceType;
    [TextArea(3, 10)]
    public string s;
}
