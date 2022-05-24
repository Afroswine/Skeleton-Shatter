using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance = null;
    public float volume = 1.00f;

    AudioSource _audioSource;

    private void Awake()
    {
        // #region/#endregion allows you to collapse the region
        #region Singleton Pattern (Simple)
        if(Instance == null)
        {
            // doesn't exist yet, this is now our singleton!
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // fill references
            _audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    public void PlaySong(AudioClip clip)
    {
        // tracking the song's time in order to seamlessly switch between drum and drumless versions
        // (the drum and drumless versions are different lengths, so there might be some discordance)
        float currentTime = _audioSource.time;
        _audioSource.clip = clip;
        _audioSource.time = currentTime;
        _audioSource.volume = volume;
        _audioSource.Play();
    }

}
