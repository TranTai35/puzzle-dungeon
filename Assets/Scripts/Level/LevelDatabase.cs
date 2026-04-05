using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDatabase", menuName ="Game/LevelDatabase")]
public class LevelDatabase : ScriptableObject
{
    public List<LevelData> levels = new();
    public int unlockedLevel = 1;
}
