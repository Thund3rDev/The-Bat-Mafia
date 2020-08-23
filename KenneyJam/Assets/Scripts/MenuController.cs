using UnityEngine;

/// <summary>
/// Class MenuController, that controls all menu behaviours
/// </summary>
public class MenuController : MonoBehaviour
{
    public static float musicVolume = 0.3f;

    /// <summary>
    /// Method Start, that executes before the first frame
    /// </summary>
    private void Start()
    {
        AudioManager.instance.ManageAudio("mainTheme", "music", "play");
    }

    /// <summary>
    /// Method SetMusicVolume, that change volume of all music in game
    /// </summary>
    /// <param name="volume">New volume value</param>
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        foreach (Audio a in AudioManager.instance.music)
        {
            a.source.volume = volume;
        }
    }

    /// <summary>
    /// Method SetSoundsVolume, that change volume of all sounds in game
    /// </summary>
    /// <param name="volume">New volume value</param>
    public void SetSoundsVolume(float volume)
    {
        foreach (Audio a in AudioManager.instance.sounds)
        {
            a.source.volume = volume;
        }
    }
}
