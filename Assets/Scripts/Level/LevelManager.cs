using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private LevelData levelData;
    [SerializeField] private Door door;
    [SerializeField] private GameObject victory;
    [SerializeField] private GameObject defeat;
    [SerializeField] private TMP_Text textActionLimit;
    [SerializeField] private Player player;
    [SerializeField] private GameObject pauseMenu;

    private int enemyKilled;
    private int actionUsed;

    private bool IsVictory;
    public bool IsDefeat;

    private void Awake()
    {

        if (Instance == null || Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;


        enemyKilled = 0;
        actionUsed = 0;

        IsVictory = false;
        IsDefeat = false;
        defeat.SetActive(IsDefeat);
        victory.SetActive(IsVictory);

        //textActionLimit.text = levelData.actionLimit.ToString();
        textActionLimit.text = $"{actionUsed}/{levelData.actionLimit}";

    }

    private void Update()
    {
        textActionLimit.text = $"Action: {actionUsed}/{levelData.actionLimit}";
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            player.SetCanPlay(false);
            pauseMenu.SetActive(true);
        }
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
        if (!IsVictory)
        {
            AudioController.Instance.PlaySoundWin();
        }

        IsVictory = true;
        victory.SetActive(IsVictory);
    }

    public void PlayDefeat()
    {
        if (actionUsed > levelData.actionLimit)
        {
            IsDefeat = true;
            defeat.SetActive(IsDefeat);
        } else if (IsDefeat == true)
        {
            defeat.SetActive(IsDefeat);
        }
    }

    public void OnClickReload(int level)
    {
        AudioController.Instance.PlaySoundClick();
        SceneManager.LoadScene($"Level{level}");
    }

    public void OnClickReturn()
    {
        AudioController.Instance.PlaySoundClick();
        player.SetCanPlay(true);
        pauseMenu.SetActive(false);

    }

    public void OnClickMenuLevel()
    {
        AudioController.Instance.PlaySoundClick();
        SceneManager.LoadScene("MenuLevel");
    }

    public void OnClickNextLevel(int level)
    {
        AudioController.Instance.PlaySoundClick();
        SceneManager.LoadScene($"Level{level}");
    }


}
