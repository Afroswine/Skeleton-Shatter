using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public interface ITargetFinder
{
    //public Collider SearchCollider; // would OnCollider() be more efficient than doing update checks? probably
    //public void OnColliderEnter();

    // Serialize these references
    string TargetTag { get; }
    float Radius { get; }

    AimConstraint AimComponent { get; }
    ConstraintSource AimSource { get; }
    bool IsSeekingTarget { get; }

    // This is a stupid way to do this, I should figure out how to structure organize Class Inheritance
    // Copy & Paste
    #region ITargetFinder Variables
    //
    /*
    
    [Header("Targeting")]
    [SerializeField] string _targetTag = "Player";
    [SerializeField] float _searchRadius = 5f;

    public string TargetTag { get { return _targetTag; } private set { _targetTag = value; } }
    public float Radius { get { return _searchRadius; } private set { _searchRadius = value; } }

    private AimConstraint _aimComponent;
    public AimConstraint AimComponent { get { return _aimComponent; } private set { _aimComponent = value; } }

    private ConstraintSource _aimSource;
    public ConstraintSource AimSource { get { return _aimSource; } private set { _aimSource = value; } }
    private bool _isSeekingTarget = true;
    public bool IsSeekingTarget { get { return _isSeekingTarget; } private set { _isSeekingTarget = value; } }
    */
    //
    #endregion ITargetFinder Variables END

    void InitializeITargetFinder();
    void SeekTarget();

    // Copy & Paste

    #region ITargetFinder Methods
    //
    /*
    public void InitializeITargetFinder()
    {
        AimComponent = GetComponent<AimConstraint>();

        _aimSource.sourceTransform = GameObject.FindWithTag(TargetTag).transform;
        _aimSource.weight = 1f;

        AimComponent.constraintActive = true;
        AimComponent.AddSource(AimSource);
        AimComponent.enabled = true;

    }

    public void SeekTarget()
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
    //
    */
    #endregion ITargetFinder Methods END

}
