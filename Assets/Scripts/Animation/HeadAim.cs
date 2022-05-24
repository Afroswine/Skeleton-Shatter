using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class HeadAim : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Skeleton _skeleton;
    [Header("Targeting")]
    [SerializeField] string _targetTag = "Player";
    [SerializeField] float _searchRadius = 5f;
    [SerializeField] MultiAimConstraint _multiAimConstraint;

    //private Skeleton _skeleton;

    private void OnEnable()
    {
        _skeleton = GetComponentInParent<Skeleton>();
        Debug.Log(_skeleton.ToString());
        _skeleton.Died.AddListener(StopAim);
        _skeleton.Reform.AddListener(StopAim);
    }

    private void OnDisable()
    {
        _skeleton.Died.RemoveListener(StopAim);
        _skeleton.Reform.RemoveListener(StopAim);
    }

    private void Start()
    {
        _skeleton = GetComponentInParent<Skeleton>();
        Debug.Log(_skeleton.ToString());
        transform.parent = GameObject.FindWithTag(_targetTag).transform;
    }

    private void Update()
    {
        float distance = Vector3.Distance(_skeleton.transform.position, transform.position);

        if (distance < _searchRadius)
        {
            _multiAimConstraint.weight = 1;
        }
        else
        {
            StopAim();
        }
    }

    private void StopAim()
    {
        _multiAimConstraint.weight = 0;
    }

    IEnumerator StopAimRoutine()
    {
        yield return new WaitForSeconds(5);
    }
}
