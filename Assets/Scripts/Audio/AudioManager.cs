using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { set; get; }

    public Audio[] audios;
    Audio engine;
    Audio trans1, trans2;

    // AdvertisementManager
    [HideInInspector]
    public int continuousAd;

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Audio a in audios)
        {
            if (a.clip)
            {
                a.source = gameObject.AddComponent<AudioSource>();
                a.source.clip = a.clip;

                a.source.volume = a.volume;
                a.source.pitch = a.pitch;
                a.source.loop = a.loop;
            }
        }
        engine = Array.Find(audios, audio => audio.name == "Engine");
        trans1 = Array.Find(audios, audio => audio.name == "Transition1");
        trans2 = Array.Find(audios, audio => audio.name == "Transition2");
        continuousAd = 1;
    }

    void Start()
    {
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            ContinueMusic();
        }
        else
        {
            StopMusic();
        }

        if (PlayerPrefs.GetInt("Soundeffects") == 1)
        {
            UnStopSoundeffects();
        }
        else
        {
            StopSoundeffects();
        }
    }

    public void Play(string name)
    {
        Audio a = Array.Find(audios, audio => audio.name == name);
        if (a == null)
        {
            Debug.LogWarning("Audio: " + name + " not found!");
            return;
        }
        a.source.Play();
    }

    public void StopMusic()
    {
        Audio music = Array.Find(audios, audio => audio.name == "Music");
        music.source.volume = 0f;
    }
    public void ContinueMusic()
    {
        Audio music = Array.Find(audios, audio => audio.name == "Music");
        music.source.volume = music.volume;
        if (!music.source.isPlaying)
            Play("Music");
    }

    public void StopSoundeffects()
    {
        foreach (Audio a in audios)
            if (a.name != "Music")
                a.source.volume = 0f;
    }
    public void UnStopSoundeffects()
    {
        foreach (Audio a in audios)
            if (a.name != "Music")
                a.source.volume = a.volume;
    }

    public void HandleEngineSound(bool boost)
    {
        if (boost)
            engine.source.volume = 0.1f;
        else
            engine.source.volume = engine.volume;

        if (!engine.source.isPlaying)
            engine.source.Play();
    }
    public void EngineSoundOff()
    {
        engine.source.volume = 0f;
    }

    public bool TransitionNotPlaying()
    {
        return !trans1.source.isPlaying && !trans2.source.isPlaying;
    }
}
