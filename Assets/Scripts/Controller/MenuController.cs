using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject menuSetting;


    private void Awake()
    {
        menuSetting.SetActive(false);
    }
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
}
