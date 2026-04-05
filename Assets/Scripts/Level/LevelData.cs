using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    public int levelIndex;
    public string sceneName;
    public int enemyCount;
    public int actionLimit;
}
