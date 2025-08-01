using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sounds[] sounds;

    public AudioSource musicAudioSource;

    // Start is called before the first frame update
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

        foreach (Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        PlayMainMenuMusic();
    }

    public void Play(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void PlayMainMenuMusic()
    {
        if(musicAudioSource != null)
        {
            musicAudioSource.Stop();
        }
        Sounds s = Array.Find(sounds, sound => sound.name == "MainMenu");
        musicAudioSource = s.source;
        musicAudioSource.Play();
        
    }
    public void PlayLevelMusic()
    {
        if (musicAudioSource != null)
        {
            musicAudioSource.Stop();
        }
        Sounds s = Array.Find(sounds, sound => sound.name == "Nivel");
        musicAudioSource = s.source;
        musicAudioSource.Play();
    }
}
