using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.ManageAudio("mainTheme", "music", "play");
    }

    // Plays the button sound
    public void ButtonSound()
    {
        AudioManager.instance.ManageAudio("buttonSound", "sound", "play");
    }

    //Set the music volume.
    public void SetMusicVolume(float volume)
    {
        foreach (Audio a in AudioManager.instance.music)
        {
            a.source.volume = volume;
        }
    }

    //Set the sounds volume.
    public void SetSoundsVolume(float volume)
    {
        foreach (Audio a in AudioManager.instance.sounds)
        {
            a.source.volume = volume;
        }
    }
}
