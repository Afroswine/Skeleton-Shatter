using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipRandomizer : MonoBehaviour
{
    [SerializeField, Tooltip("If true, randomize the current clip on Awake.")]
    private bool _randomizeOnAwake = true;
    [SerializeField, Tooltip("The audio clips to pick between.")]
    private AudioClip[] _clips;
    
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_randomizeOnAwake)
        {
            RandomizeAndPlay();
        }
    }

    public void RandomizeAndPlay()
    {
        int index = Mathf.RoundToInt(Random.Range(0, _clips.Length));

        Debug.Log(index);
        _audioSource.clip = _clips[index];

        _audioSource.Play();
    }
}
