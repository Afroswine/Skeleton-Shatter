using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class Skeleton : Enemy, ITargetFinder
{
    #region Variables
    //

    #region Skeleton Variables
    //
    [Header("Skeleton")]
    [SerializeField] Collider _worldCollider;
    [SerializeField] bool _reviving = true;
    [SerializeField] float _brokenDuration = 5f;
    public float BrokenDuration => _brokenDuration;
    private SkeletonAnimations _skeletonAnimations;
    [Header("Targeting")]
    [SerializeField] bool _isSeekingTarget = true;
    [SerializeField] string _targetTag = "Player";
    [SerializeField] float _searchRadius = 5f;
    [Header("Break Explosion")]
    [SerializeField] float _explosionForce = 150f;
    [SerializeField] float _explosionRadius = 5f;
    [Header("Bones")]
    [SerializeField] float _physicsDuration = 3f;
    [SerializeField] GameObject[] _bones;
    //[SerializeField] GameObject _deathBones;
    public GameObject[] Bones => _bones;

    public UnityEvent Reform;

    private Rigidbody _mainRB;

    //
    #endregion Skeleton Variables End

    #region ITargetFinder Variables
    //
    public string TargetTag { get { return _targetTag; } private set { _targetTag = value; } }
    public float Radius { get { return _searchRadius; } private set { _searchRadius = value; } }

    private AimConstraint _aimComponent;
    public AimConstraint AimComponent { get { return _aimComponent; } private set { _aimComponent = value; } }

    private ConstraintSource _aimSource;
    public ConstraintSource AimSource { get { return _aimSource; } private set { _aimSource = value; } }
    public bool IsSeekingTarget { get { return _isSeekingTarget; } private set { _isSeekingTarget = value; } }
    //
    #endregion ITargetFinder Variables END

    //
    #endregion Variables END

    #region ITargetFinder Methods
    //
    public void InitializeITargetFinder()
    {
        AimComponent = GetComponent<AimConstraint>();
        _aimSource.sourceTransform = GameObject.FindWithTag(TargetTag).transform;
        _aimSource.weight = 1f;
        AimComponent.constraintActive = true;
        AimComponent.AddSource(AimSource);

        if (IsSeekingTarget)
        {
            AimComponent.enabled = true;
        }

    }

    public void SeekTarget()
    {
        if (IsSeekingTarget)
        {
            float distance = Vector3.Distance(AimSource.sourceTransform.transform.position, transform.position);

            if (distance < _searchRadius)
            {
                AimComponent.enabled = true;
            }
            else
            {
                AimComponent.enabled = false;
            }
        }
        else
        {
            AimComponent.enabled = false;
        }
        
    }
    //
    #endregion ITargetFinder Methods END

    private void OnDisable()
    {
        _skeletonAnimations.Reformed.RemoveListener(Revive);
    }

    private void Start()
    {
        _mainRB = GetComponent<Rigidbody>();
        _skeletonAnimations = GetComponent<SkeletonAnimations>();
        _skeletonAnimations.Reformed.AddListener(Revive);
        InitializeITargetFinder();

        if (_physicsDuration > _brokenDuration)
        {
            _physicsDuration = _brokenDuration;
        }
    }

    private void Update()
    {
        SeekTarget();
    }

    #region Death & Revival
    //

    #region Death
    //
    public override void Die()
    {
        _worldCollider.enabled = false;

        BoneBreak();

        IsSeekingTarget = false;
        _mainRB.isKinematic = true;
        AimComponent.enabled = false;

        if (_reviving == true)
        {
            DestroyOnDeath = false;
            _reviving = false;
        }
        else
        {
            DestroyOnDeath = true;
            for(int i = 0; i < Bones.Length; i++)
            {
                Instantiate(Bones[i].transform.gameObject, transform.position, transform.rotation);
            }
            CreateExplosiveForce();
        }

        base.Die();
    }

    private void CreateExplosiveForce()
    {
        float range = 1.1f;
        Vector3 variance = new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));

        Collider[] colliders = Physics.OverlapSphere(DeathFxOrigin.position + variance, _explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(_explosionForce, DeathFxOrigin.position , _explosionRadius, 1.0f);
            }
        }
    }

    private void BoneBreak()
    {
        ToggleLayerCollision();
        StartCoroutine(BrokenDurationRoutine());

        for (int i = 0; i < _bones.Length; i++)
        {
            _bones[i].GetComponent<MeshCollider>().isTrigger = false;

            if (_bones[i].TryGetComponent<ParentConstraint>(out ParentConstraint parentConstraint))
            {
                ConstraintSource constraint1 = parentConstraint.GetSource(0);
                constraint1.weight = 0;
                parentConstraint.constraintActive = false;
            }

            
            if(_bones[i].GetComponent<Rigidbody>() == false)
            {
                _bones[i].AddComponent<Rigidbody>();
            }
            if(_bones[i].TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                rigidbody.mass = _bones[i].GetComponent<SkeletonBone>().Mass;
            }

            StartCoroutine(PhysicsDurationRoutine(_bones[i]));
        }


        CreateExplosiveForce();

        //Debug.Log("BoneBreak()");
    }
    //
    #endregion Death END

    #region Revival
    //
    private void Revive()
    {
        IsSeekingTarget = true;
        _mainRB.isKinematic = false;
        _worldCollider.enabled = true;
        ToggleLayerCollision();

        for(int i = 0; i < _bones.Length; i++)
        {
            _bones[i].GetComponent<MeshCollider>().isTrigger = true;
            _bones[i].GetComponent<MeshCollider>().enabled = true;
        }
    }
    //
    #endregion Revival END

    #region IEnumerator Timers
    //
    IEnumerator PhysicsDurationRoutine(GameObject bone)
    {
        yield return new WaitForSeconds(_physicsDuration);
        Destroy(bone.GetComponent<Rigidbody>());
        bone.GetComponent<MeshCollider>().enabled = false;
    }

    IEnumerator BrokenDurationRoutine()
    {
        yield return new WaitForSeconds(_brokenDuration);
        Reform.Invoke();
    }
    //
    #endregion IEnumerator Timers END

    private void ToggleLayerCollision()
    {

        Physics.IgnoreLayerCollision(0, 10, true);
        Physics.IgnoreLayerCollision(8, 8, false);
    }

    //
    #endregion Death & Revival END




}
