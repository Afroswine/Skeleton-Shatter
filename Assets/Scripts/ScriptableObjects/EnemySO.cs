using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EnemySO", menuName = "ScriptableObjects/Enemy")]
public class EnemySO : ScriptableObject
{
    [Header("Stats")]
    [SerializeField, Tooltip("A value of 0 results in Death.")]
    private int _health = 100;
    public int Health => _health;
    [SerializeField, Tooltip("Movement Speed.")]
    private float _speed = 5f;
    public float Speed => _speed;

    [Header("Special Conditions")]
    [SerializeField, Tooltip("If true, this enemy cannot be killed.")] 
    private bool _isEssential = false;
    public bool IsEssential => _isEssential;

    [System.NonSerialized]
    public UnityEvent HurtEvent;
    [System.NonSerialized]
    public UnityEvent DiedEvent;
    [System.NonSerialized]
    public UnityEvent HealedEvent;
    [System.NonSerialized]
    public UnityEvent HealthChangedEvent;


}
