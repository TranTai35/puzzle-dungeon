using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject menuSetting;
    private void Awake()
    {
        menuSetting.SetActive(false);
    }

    public void OnClickPlayButton()
    {
        AudioController.Instance.PlaySoundClick();
        SceneManager.LoadScene("MenuLevel");
        
    }

    public void OnClickSettingButton()
    {
        AudioController.Instance.PlaySoundClick();
        menuSetting.SetActive(true);
        

    }

    public void OnClickCloseSetting()
    {
        AudioController.Instance.PlaySoundClick();
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
