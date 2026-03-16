using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private LevelData levelData;
    [SerializeField] private Door door;
    [SerializeField] private GameObject victory;

    private int enemyKilled = 0;
    private int actionUsed = 0;

    private bool IsVictory;

    private void Awake()
    {
        Instance = this;
        IsVictory = false;
        victory.SetActive(IsVictory);
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

    public void PlayVictory()
    {
        IsVictory = true;
        victory.SetActive(IsVictory);
    }
}
