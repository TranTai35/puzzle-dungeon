using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuLevelController : MonoBehaviour
{
    public LevelDatabase levelDatabase;
    public GameObject buttonPrefab;
    public Transform contentParent;

    private string unLockedLevel = "UnlockedLevel";

    private void Start()
    {
        LoadProgress();
        GenerateButtons();
    }

    private void LoadProgress()
    {
        levelDatabase.unlockedLevel = PlayerPrefs.GetInt(unLockedLevel, 1);
        
    }

    private void GenerateButtons()
    {
        for (int i = 0; i < levelDatabase.unlockedLevel && i < levelDatabase.levels.Count; i++)
        {
            LevelData data = levelDatabase.levels[i];

            GameObject btnObj = Instantiate(buttonPrefab, contentParent);

            TextMeshProUGUI txt = btnObj.GetComponentInChildren<TextMeshProUGUI>();
            txt.text = "Level " + data.levelIndex;

            Button btn = btnObj.GetComponent<Button>();

            int index = i;

            btn.onClick.AddListener(() => OnClickPlayButton(index));
        }
    }

    private void OnClickPlayButton(int index)
    {
        SceneManager.LoadScene(levelDatabase.levels[index].sceneName);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            SceneManager.LoadScene("Menu");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Reset data!");
        }
    }
}
