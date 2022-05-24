using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Enemy : MonoBehaviour, IHealth
{
    [Header("Stats")]
    [SerializeField] int _maxHealth;
    [SerializeField] int _startingHealth;
    [SerializeField] bool _isEssential = false;
    [SerializeField] bool _destroyOnDeath = false;
    public bool DestroyOnDeath { get { return _destroyOnDeath; } set { _destroyOnDeath = value; } }
    [Header("Visuals")]
    [SerializeField] private GameObject _spawnFX;
    [SerializeField] private GameObject _deathFX;
    [SerializeField] private Transform _deathFXOrigin;
    public Transform DeathFxOrigin => _deathFXOrigin;

    public UnityEvent TookDamage;
    public UnityEvent Healed;
    public UnityEvent HealthChanged;
    public UnityEvent Died;
    private LevelController _levelController;

    #region IHealth
    // IHealth Begin
    public int MaxHealth { get; private set; }
    public int StartingHealth { get; private set; }
    private int _currentHealth;
    public int CurrentHealth { get { return _currentHealth; } private set { _currentHealth = value; } }
    public bool IsEssential { get { return _isEssential; } private set { _isEssential = value; } }

    public virtual void ApplyDamage(int damage)
    {
        CurrentHealth -= Mathf.Abs(damage);
        TookDamage.Invoke();

        if(CurrentHealth <= 0)
        {
            if (!IsEssential)
            {
                Die();
            }
        }
    }

    public virtual void Heal(int amount)
    {
        CurrentHealth += Mathf.Abs(amount);
        Healed.Invoke();

        if(CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        
        Debug.Log("Healed: " + amount);
    }

    public void SetHealth(int amount)
    {
        CurrentHealth = amount;
        HealthChanged.Invoke();

        if(CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }

    public virtual void Die()
    {
        _levelController.IncreaseKillCount();
        Died.Invoke();

        if (_deathFX != null)
        {
            Instantiate(_deathFX, _deathFXOrigin.position, _deathFXOrigin.rotation);
        }

        if (_destroyOnDeath)
        {
            Destroy(gameObject);
        }
    }

    public void DieAndDestroy()
    {
        Die();
        Destroy(gameObject);
    }
    //
    #endregion IHealth ***End***

    #region OnEnable/OnDisable
    // EnableDisable Start
    private void OnEnable()
    {
        _levelController = GameObject.FindWithTag("LevelController").GetComponent<LevelController>();
        MaxHealth = _maxHealth;
        CurrentHealth = _startingHealth;
        IsEssential = _isEssential;

        

        if (!IsEssential)
        {
            //_levelController.RespawnEnemy.AddListener(WillDestroyOnDeath);
            _levelController.RespawnEnemy.AddListener(DieAndDestroy);
        }
    }

    private void OnDisable()
    {
        if (!IsEssential)
        {
            _levelController.RespawnEnemy.RemoveListener(DieAndDestroy);
            //_levelController.RespawnEnemy.RemoveListener(WillDestroyOnDeath);
        }
    }
    // EnableDisable End
    #endregion OnEnable/OnDisable End

    private void Awake()
    {
        _levelController = GameObject.FindWithTag("LevelController").GetComponent<LevelController>();
        if (_spawnFX != null)
        {
            GameObject spawnFX = _spawnFX;
            float multiplier = transform.lossyScale.x / spawnFX.transform.localScale.x;
            spawnFX.transform.localScale = transform.lossyScale;
            Instantiate(spawnFX, transform);
        }
    }
}
