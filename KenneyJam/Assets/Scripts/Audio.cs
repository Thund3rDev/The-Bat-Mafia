using UnityEngine;

/// <summary>
/// Class Audio, which contains all necesary info of a sound 
/// </summary>
[System.Serializable]
public class Audio
{
    [Tooltip("Audio name")]
    public string name;
    [Tooltip("Clip to play")]
    public AudioClip clip;

    [Tooltip("Audio volume")]
    [Range(0f, 1f)]
    public float volume = 1;
    [Tooltip("Audio pitch")]
    [Range(.1f, 3f)]
    public float pitch = 1;

    [Tooltip("Bool that indicates if loops")]
    public bool loop;

    [HideInInspector]
    [Tooltip("AudioSource to play this on")]
    public AudioSource source;
}
