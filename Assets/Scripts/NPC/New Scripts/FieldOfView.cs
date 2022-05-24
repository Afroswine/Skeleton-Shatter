using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// For GUI, see the 'FieldOfViewEditor' script
public class FieldOfView : MonoBehaviour
{
    [Header("Field of View")]
    [SerializeField, Tooltip("The origin of the view. Use the head or eyes.")]
    private Transform _viewOrigin;
    public Transform ViewOrigin => _viewOrigin;
    [SerializeField, Tooltip("(float) The range of vision.")]
    private float _radius = 5f;
    public float Radius => _radius;
    [SerializeField, Range(0, 360), Tooltip("(float) The angle of vision.")]
    private float _angle = 90f;
    public float Angle => _angle;
    [Tooltip("Time (seconds) inbetween FOV checks.")]
    private float _checkDelay = 0.2f;
    public float CheckDelay => _checkDelay;

    [Header("Target Information")]
    [SerializeField, Tooltip("React to GameObjects with this tag.")]
    private string _targetTag = "Player";
    private GameObject _target;
    public GameObject Target => _target;

    [Header("Filtering")]
    [SerializeField, Tooltip("The layers to search for targets.")]
    private LayerMask _targetMask;
    public LayerMask TargetMask => _targetMask;
    [SerializeField, Tooltip("The layers that block line of sight.")]
    private LayerMask _obstructionMask;
    public LayerMask ObstructionMask => _obstructionMask;

    // If target is within FOV, and the view is unobstructed, this is true.
    private bool _canSeeTarget;
    public bool CanSeeTarget => _canSeeTarget;

    [HideInInspector]
    public UnityEvent TargetSpottedEvent;   // Invoked when the target is initially spotted/re-spotted

    private void Start()
    {
        // Debugging
        if(_viewOrigin == null)
        {
            _viewOrigin = gameObject.transform;
            Debug.Log(gameObject.name + "'s \"FieldOfView()._viewOrigin\" has not been set.");
        }
        if (_targetMask == 0)
        {
            Debug.Log(gameObject.name + "'s \"FieldOfView()._targetMask\" has not been set.");
        }
        if (_obstructionMask == 0)
        {
            Debug.Log(gameObject.name + "'s \"FieldOfView()._obstructionMask\" has not been set.");
        }

        // Begin searching for target
        _target = GameObject.FindGameObjectWithTag(_targetTag);
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new (_checkDelay);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        // Finds all objects within the OverlapSphere that are within the _targetMask layer(s).
        Collider[] rangeChecks = Physics.OverlapSphere(_viewOrigin.position, _radius, _targetMask);

        // Only uses the first object in the array. THIS MIGHT NEED TO BE CHANGED.
        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - _viewOrigin.position).normalized;  // .normalize caps the vector's length

            // If target is within the viewing angle
            if (Vector3.Angle(_viewOrigin.forward, directionToTarget) < _angle / 2)
            {
                float distanceToTarget = Vector3.Distance(_viewOrigin.position, target.position);

                // If the raycast does not hit the obstruction mask
                if (!Physics.Raycast(_viewOrigin.position, directionToTarget, distanceToTarget, _obstructionMask))
                {
                    if (!_canSeeTarget)
                        TargetSpottedEvent.Invoke();

                    _canSeeTarget = true;
                }
                else
                    _canSeeTarget = false;
            }
            else
                _canSeeTarget = false;
        }
        else if (_canSeeTarget)
            _canSeeTarget = false;
    }
}
