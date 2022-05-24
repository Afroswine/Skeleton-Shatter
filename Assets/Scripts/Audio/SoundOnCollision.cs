using System.Collections;
using UnityEngine;

public class SoundOnCollision : MonoBehaviour
{
    private AudioSource _audioSource;
    private bool _canPlay = true;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_canPlay)
        {
            _audioSource.Play();
            StartCoroutine(WaitRoutine());
        }
    }

    private IEnumerator WaitRoutine()
    {
        _canPlay = false;
        yield return new WaitForSeconds(_audioSource.clip.length);
        _canPlay = true;
    }
}
