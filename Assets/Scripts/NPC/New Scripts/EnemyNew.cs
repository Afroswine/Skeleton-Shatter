using UnityEngine;
using UnityEngine.Events;

public class EnemyNew : MonoBehaviour, IHealthNew
{
    [Header("Stats")]
    [SerializeField, Tooltip("(int) Maximum health")]
    private int _maxHealth = 100;
    [SerializeField, Tooltip("(int) Current health.")]
    private int _health = 100;
    [SerializeField, Tooltip("(float) Movement speed.")]
    private float _moveSpeed = 5f;
    [SerializeField, Tooltip("(bool) If true, do not invoke DiedEvent when health reaches 0.")]
    private bool _isEssential = false;

    [Header("Targeting")]
    [SerializeField, Tooltip("Determines what the enemy reacts to and when.")]
    private EnemyFOV _enemyFOV;

    [Header("Physics & Collisions")]
    [SerializeField, Tooltip("The rigidbody this enemy uses.")]
    private Rigidbody _rigidbody;
    [SerializeField, Tooltip("Used for environmental collisions.")]
    private Collider _worldCollider;

    //[System.NonSerialized]
    [HideInInspector]
    public UnityEvent HurtEvent;                // Called when damage taken
    [HideInInspector]
    public UnityEvent HealthUpdatedEvent;       // Called when health changed
    [HideInInspector]
    public UnityEvent DiedEvent;                // Called when health <= 0

    #region IHealthNew Properties
    //
    public int MaxHealth
    {
        get { return _maxHealth; }
        private set { _maxHealth = value; }
    }
    public int Health
    {
        get { return _health; }
        private set { _health = value; }
    }
    //
    #endregion

    #region IHealthNew Methods
    //
    public virtual void SetHealth(int health)
    {
        _health = Mathf.Clamp(health, 0, _maxHealth);

        HealthUpdatedEvent.Invoke();
    }

    public virtual void ApplyDamage(int amount, Vector3 point)
    {
        int health = _health - Mathf.Abs(amount);
        _health = Mathf.Clamp(health, 0, _maxHealth);

        HurtEvent.Invoke();
        HealthUpdatedEvent.Invoke();

        if(_health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if (!_isEssential)
        {
            DiedEvent.Invoke();
        }
        else
        {
            SetHealth(1);
        }
    }
    //
    #endregion

    private void Awake()
    {
        // Ensure that _health is not higher than _maxHealth
        _health = Mathf.Clamp(_health, 0, _maxHealth);
    }

    private void OnEnable()
    {
        if (_enemyFOV)
        {
            _enemyFOV.BeginPursuitEvent.AddListener(PathToTarget);
            //_enemyFOV.EndPursuitEvent.AddListener();
        }
    }

    private void OnDisable()
    {
        if (_enemyFOV)
        {
            _enemyFOV.BeginPursuitEvent.RemoveListener(PathToTarget);
            //_enemyFOV.EndPursuitEvent.RemoveListener();
        }
    }

    private void PathToTarget()
    {

    }
}
