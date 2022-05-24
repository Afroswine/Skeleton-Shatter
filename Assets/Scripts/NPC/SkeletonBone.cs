using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SkeletonBone : MonoBehaviour
{
    [SerializeField] float _mass = 0.25f;
    public float Mass => _mass;
    private Skeleton _skeleton;
    private SkeletonAnimations _skeletonAnim;

    private void OnEnable()
    {
        if(_skeleton != null)
        {
            _skeleton = GetComponentInParent<Skeleton>();
            _skeletonAnim = GetComponentInParent<SkeletonAnimations>();
            _skeleton.Died.AddListener(DeactivateComponents);
            _skeleton.Reform.AddListener(ActivateComponents);
        }
        
    }

    private void OnDisable()
    {
        if(_skeleton != null)
        {
            _skeleton.Died.RemoveListener(DeactivateComponents);
            _skeleton.Reform.AddListener(ActivateComponents);
        }
    }

    private void Start()
    {
        Physics.IgnoreLayerCollision(0, 10, true);
        Physics.IgnoreLayerCollision(8, 8, false);

    }

    private void DeactivateComponents()
    {
        StartCoroutine(DeactivateRoutine());
    }

    private void ActivateComponents()
    {
        GetComponent<MeshCollider>().enabled = true;
        GetComponent<AudioSource>().enabled = true;
    }

    IEnumerator DeactivateRoutine()
    {
        yield return new WaitForSeconds(_skeleton.BrokenDuration);
        Destroy(GetComponent<Rigidbody>());
        GetComponent<MeshCollider>().enabled = false;
        GetComponent<AudioSource>().enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            audioSource.Play();
        }
    }

}
