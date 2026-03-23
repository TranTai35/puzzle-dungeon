using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private AudioClip soundWin;
    [SerializeField] private AudioClip soundLose;


    private const string SceneMenu = "Menu";
    private const string SceneLevel = "Level";


    private void Awake()
    {
        if (Instance == null || Instance != this)
        {
            Destroy(Instance);
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
        if (string.Equals(scene.name, SceneMenu))
        {
            PlayBGM(soundMenu);
        }

        if (string.Equals(scene.name, "Level1"))
        {
            PlayBGM(soundGame);
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
        //if (audioSourceSFM.clip != soundHumanWalk)
        //{
        //    audioSourceSFM.clip = soundHumanWalk;
        //}

        //if (!audioSourceSFM.isPlaying)
        //{
        //    audioSourceSFM.Play();
        //}


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
        audioSourceSFM.PlayOneShot(soundWin);
    }

    public void PlaySoundLose()
    {
        audioSourceSFM.PlayOneShot(soundLose);
    }
}
