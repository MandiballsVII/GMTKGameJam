using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("-----AudioSource-----")]
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource SFXSource;

    [Header("-----Music Clips-----")]
    public AudioClip mainMenu;
    public AudioClip theSafeZone;
    public AudioClip level;

    [Header("-----Sound Clips-----")]
    public AudioClip salto;
    public AudioClip muerte;
    public AudioClip cogerItem;
    public AudioClip dash;
    public AudioClip ataque;
    public AudioClip sandBallThrow;
    public AudioClip sandBallExplotion;
    public AudioClip polymorf;
    public AudioClip enemyAttack;
    public AudioClip enemyDeath;
    public AudioClip breakingWalls;
    public AudioClip capsuleBreak;
    public AudioClip openDoor;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        musicSource.clip = mainMenu;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayTheSafeZone()
    {
        musicSource.Stop();
        musicSource.clip = level;
        musicSource.Play();
    }

    public void PlayLevelMusic()
    {
        musicSource.Stop();
        musicSource.clip = level;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void PlayMainMenuMusic()
    {
        musicSource.Stop();
        musicSource.clip = mainMenu;
        musicSource.Play();
    }
}
