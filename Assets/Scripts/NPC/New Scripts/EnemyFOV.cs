using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyFOV : FieldOfView
{
    [Header("Pursuit")]
    [SerializeField, Tooltip("If a target leaves this range, immediately stop pursuing them.")]
    private float _maxPursuitRadius = 10f;
    public float MaxPursuitRadius => _maxPursuitRadius;
    [SerializeField, Tooltip("How long (in seconds) line of sight must be broken to stop pursuing a target.")]
    private float _pursuitConfidence = 5f;
    [Tooltip("Counts up towards _pursuitConfidence when the target stops being seen.")]
    private float _currentConfidence = 0f;

    private Vector3 _pursuitOrigin;   // Stores the transform of this object when it spawns. Used as the origin of the _maxPursuitRadius
    public Vector3 PursuitOrigin => _pursuitOrigin;
    private bool _inPursuit = false;    // If true, this enemy is pursuing a target that had been seen
    public bool InPursuit => _inPursuit;

    [HideInInspector]
    public UnityEvent BeginPursuitEvent;
    [HideInInspector]
    public UnityEvent EndPursuitEvent;

    private void Awake()
    {
        _pursuitOrigin = gameObject.transform.position;
        _maxPursuitRadius = Mathf.Max(_maxPursuitRadius, Radius);
    }

    private void OnEnable()
    {
        base.TargetSpottedEvent.AddListener(BeginPursuit);
    }

    private void OnDisable()
    {
        base.TargetSpottedEvent.RemoveListener(BeginPursuit);
    }

    private void BeginPursuit()
    {
        if (!_inPursuit)
        {
            StartCoroutine(PursuitRoutine());
        }
    }

    private IEnumerator PursuitRoutine()
    {
        WaitForSeconds wait = new(CheckDelay);
        _inPursuit = true;
        BeginPursuitEvent.Invoke();

        while (_inPursuit)
        {
            yield return wait;
            PursuitCheck();
        }

        EndPursuitEvent.Invoke();
    }

    private void PursuitCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(_pursuitOrigin, _maxPursuitRadius, TargetMask);

        // end pursuit if target breaks line of sight for the specified time
        if (!CanSeeTarget)
        {
            _currentConfidence += Time.deltaTime + CheckDelay;

            if(_currentConfidence >= _pursuitConfidence)
            {
                _inPursuit = false;
            }
        }
        else
        {
            _currentConfidence = 0f;
        }

        // end pursuit if target leaves the radius
        if(rangeChecks.Length == 0)
        {
            _inPursuit = false;
        }
    }
}
