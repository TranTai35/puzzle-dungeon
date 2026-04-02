using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject menuSetting;
    //[SerializeField] private List<Button> listButton = new();


    private void Awake()
    {

        //InitButton();
        menuSetting.SetActive(false);

    }

    //private void InitButton()
    //{
    //    if (listButton == null || listButton.Count == 0) return;
    //    for (int i = 0; i < listButton.Count; i++)
    //    {
    //        listButton[i].onClick.AddListener(() => OnClickPlayLevel(i));
    //    }
    //}

    //private void RemoveButtonListener()
    //{
    //    if (listButton == null || listButton.Count == 0) return;
    //    for (int i = 0; i < listButton.Count; i++)
    //    {
    //        listButton[i].onClick.RemoveListener(() => OnClickPlayLevel(i));
    //    }
    //}

    //private void OnDestroy()
    //{
    //    RemoveButtonListener();
    //}

    public void OnClickPlayButton()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OnClickSettingButton()
    {
        menuSetting.SetActive(true);
    }

    public void OnClickCloseSetting()
    {
        menuSetting.SetActive(false);
    }

    public void OnClickExitButton()
    {
        Application.Quit();
    }

    //public void OnClickPlayLevel(int level)
    //{
    //    SceneManager.LoadScene($"Level{level}");
    //}
}
