using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Tutorial", menuName = "Tutorial/TutorialData")]
public class TutorialData : ScriptableObject
{
    [field: SerializeField] public List<TutorialSentence> sentenceList = new();
}
