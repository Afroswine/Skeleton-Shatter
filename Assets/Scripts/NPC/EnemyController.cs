using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] 
    private GameObject _player;
    [SerializeField] 
    private EnemyScriptableObject _enemySO;
    /*
    [SerializeField] 
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource.clip = _enemySO.EnemyAttackType.AttackSound;
    }
    */
}
