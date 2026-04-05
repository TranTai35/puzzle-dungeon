using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;
    [SerializeField] private AudioSource audioSourceBGM;
    [SerializeField] private AudioSource audioSourceSFM;


    [SerializeField] private AudioClip soundMenu;
    [SerializeField] private AudioClip soundGame;
    [SerializeField] private AudioClip soundHumanWalk;
    [SerializeField] private AudioClip soundPick;
    [SerializeField] private AudioClip soundSelect;
    [SerializeField] private AudioClip soundAttack;
    [SerializeField] private AudioClip soundHitDame;
    [SerializeField] private AudioClip soundDoor;
    [SerializeField] private AudioClip soundLose;
    [SerializeField] private AudioClip soundWin;
    [SerializeField] private AudioClip soundClick;


    private const string SceneMenu = "Menu";
    private const string SceneLevel = "Level";


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name.Length >= 5)
        {
            string level = scene.name.Substring(0, 5);
            if (string.Equals(level, "Level"))
            {
                PlayBGM(soundGame);
            }
        }
        
        if(scene.name.Length >= 4)
        {
            string menu = scene.name.Substring(0, 4);
            if (string.Equals(menu, "Menu"))
            {
                PlayBGM(soundMenu);
            }
        }
       

    }

    private void PlayBGM(AudioClip audioClip)
    {
        if (audioSourceBGM == null || audioClip == null)
        {
            return;
        }

        audioSourceBGM.loop = true;
        audioSourceBGM.clip = audioClip;
        audioSourceBGM.Play();
    }

    public void PlayHumanWalk()
    {
    
        audioSourceSFM.PlayOneShot(soundHumanWalk);
    }

    public void PlayHumanPick()
    {
        audioSourceSFM.PlayOneShot(soundPick);
    }
   
    public void PlaySoundSelect()
    {
        audioSourceSFM.PlayOneShot(soundSelect);
    }

    public void PlaySoundAttack()
    {
        audioSourceSFM.PlayOneShot(soundAttack);
    }

    public void PlayHitDame()
    {
        audioSourceSFM.PlayOneShot(soundHitDame);
    }

    public void PlaySoundDoor()
    {
        audioSourceSFM.PlayOneShot(soundDoor);
    }
    public void PlaySoundWin()
    {
        //audioSourceBGM.Stop();
        audioSourceSFM.PlayOneShot(soundWin);
    }

    public void PlaySoundLose()
    {
        //audioSourceBGM.Stop();
        audioSourceSFM.PlayOneShot(soundLose);
    }

    public void PlaySoundClick()
    {
        audioSourceSFM.PlayOneShot(soundClick);
    }

    
}
