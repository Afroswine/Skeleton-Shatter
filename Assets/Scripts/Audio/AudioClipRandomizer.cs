using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioClipRandomizer : MonoBehaviour
{
    [SerializeField, Tooltip("If true, play a random clip on Awake.")]
    private bool _randomizeOnAwake = true;
    [SerializeField, Tooltip("The audio clips to pick between.")]
    private AudioClip[] _clips;
    
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if(_randomizeOnAwake)
            RandomizeAndPlay();
    }

    private void RandomizeClip()
    {
        int index = Mathf.RoundToInt(Random.Range(0, _clips.Length));
        //Debug.Log(gameObject + "RandomizeClip() set clip to: _clips[" + index + "]");
        _audioSource.clip = _clips[index];
    }

    public void RandomizeAndPlay()
    {
        RandomizeClip();
        _audioSource.Play();
    }
    
}
