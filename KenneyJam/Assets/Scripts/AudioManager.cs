using System;
using UnityEngine;

/// <summary>
/// Class AudioManager, that controls all audio in the game
/// </summary>
public class AudioManager : MonoBehaviour
{
    [Tooltip("Singleton")]
    [HideInInspector]
    public static AudioManager instance;

    [Tooltip("All music in the game")]
    public Audio[] music;
    [Tooltip("All sounds in the game")]
    public Audio[] sounds;

    /// <summary>
    /// Method Awake, that executes on script load
    /// </summary>
    void Awake()
    {
        // Instance itself
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Set to don't destroy on load
        DontDestroyOnLoad(this);

        // Set the info of each audio
        foreach (Audio a in music)
        {
            a.source = gameObject.AddComponent<AudioSource>(); ;
            a.source.clip = a.clip;

            a.source.volume = a.volume;
            a.source.pitch = a.pitch;
            a.source.loop = a.loop;
        }

        foreach (Audio a in sounds)
        {
            a.source = gameObject.AddComponent<AudioSource>(); ;
            a.source.clip = a.clip;

            a.source.volume = a.volume;
            a.source.pitch = a.pitch;
            a.source.loop = a.loop;
        }
    }

    private void Start()
    {
        Audio a = Array.Find(music, sound => sound.name == "mainTheme");
        if (a == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        a.source.volume = MenuController.musicVolume * LevelChanger._instance.soundLevel;
    }

    private void Update()
    {
        Audio a = Array.Find(music, sound => sound.name == "mainTheme");
        if (a == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        a.source.volume = MenuController.musicVolume * LevelChanger._instance.soundLevel;
    }

    /// <summary>
    /// Method Manage audio, that do everything with audio
    /// </summary>
    /// <param name="name">Name of the audio</param>
    /// <param name="type">Type of audio (music/sound)</param>
    /// <param name="action">Action to do</param>
    public void ManageAudio(string name, string type, string action)
    {
        if (type == "music")
        {
            Audio a = Array.Find(music, sound => sound.name == name);
            if (a == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }
            if (action == "play")
                a.source.PlayOneShot(a.clip);
            else if (action == "stop")
                a.source.Stop();
            else if (action == "pause")
                a.source.Pause();
            else if (action == "unpause")
                a.source.UnPause();
        }
        else if (type == "sound")
        {
            Audio a = Array.Find(sounds, sound => sound.name == name);
            if (a == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }
            if (action == "play")
                a.source.PlayOneShot(a.clip);
            else if (action == "stop")
                a.source.Stop();
            else if (action == "pause")
                a.source.Pause();
            else if (action == "unpause")
                a.source.UnPause();
        }
    }

    public void PlaySound(string name)
    {
        ManageAudio(name, "sound", "play");
    }

    public void StopSound(string name)
    {
        ManageAudio(name, "sound", "stop");
    }
}
