using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkeletonSpawner : MonoBehaviour
{
    [SerializeField] Skeleton _skeleton;
    [SerializeField] Transform _origin;
    [SerializeField] float _scale = 1f;
    [SerializeField] float _delay = 0f;

    private LevelController _levelController;
    private GameObject _currentSkeleton;

    #region OnEnable/OnDisable
    private void OnEnable()
    {
        if (GameObject.FindWithTag("LevelController").TryGetComponent<LevelController>(out LevelController levelController))
        {
            _levelController = levelController;

            if (_skeleton.IsEssential == false)
            {
                _levelController.RespawnEnemy.AddListener(Spawn);
            }
        }

        Spawn();
    }

    private void OnDisable()
    {
        if (_skeleton.IsEssential == false)
        {
            _levelController.RespawnEnemy.RemoveListener(Spawn);
        }
    }
    #endregion OnEnable/OnDisable

    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(_delay);
        _currentSkeleton = Instantiate(_skeleton.gameObject, _origin.position, _origin.rotation);
        _currentSkeleton.transform.localScale = new Vector3(_scale, _scale, _scale);

    }

    public void Spawn()
    {
        StartCoroutine(RespawnRoutine());
    }

}
