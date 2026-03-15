using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private LevelData levelData;
    [SerializeField] private Door door;

    private int enemyKilled = 0;
    private int actionUsed = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void EnemyKilled()
    {
        enemyKilled++;

        if (enemyKilled == levelData.enemyCount)
        {
            door.OpenDoor();
        }
    }

    public void UseAction()
    {
        actionUsed++;

        if (actionUsed > levelData.actionLimit)
        {
            Debug.Log("Hết lượt hành động");
        }
    }
}
