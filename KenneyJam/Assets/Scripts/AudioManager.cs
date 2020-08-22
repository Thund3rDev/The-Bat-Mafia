using System;
using UnityEngine;

// Class AudioManager, that controls all audio in the game.
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Audio[] music;
    public Audio[] sounds;

    // On Awake, instance itselft and if there is another, destroy it.
    // Then, set to don't destroy on load.
    // Also gets all the data from the sounds.
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);

        foreach (Audio a in music)
        {
            a.source = gameObject.AddComponent<AudioSource>();
            a.source.clip = a.clip;

            a.source.volume = a.volume;
            a.source.pitch = a.pitch;
            a.source.loop = a.loop;
        }

        foreach (Audio a in sounds)
        {
            a.source = gameObject.AddComponent<AudioSource>();
            a.source.clip = a.clip;

            a.source.volume = a.volume;
            a.source.pitch = a.pitch;
            a.source.loop = a.loop;
        }
    }

    // If we need to do something on a sound, we call this function.
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
}
