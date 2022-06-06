using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : MonoBehaviour
{
    [Header("Transform Information for Raycast")]
    [Tooltip("The raycast's forward direction will be set to this.")]
    [SerializeField] Camera _cameraController;
    [SerializeField] Transform _rayOrigin;
    [SerializeField] Transform _barrelLocator;

    [Header("Weapon Stats")]
    [SerializeField] float _shootDistance = 10f;
    [SerializeField] int _weaponDamage = 25;
    [SerializeField] float _roundsPerSecond = 8f;
    [SerializeField] float _maxAngleOfInaccuracy = 0.0f;
    [SerializeField] float _knockbackMultiplier = 1f;
    [SerializeField] float _explosionForce = 5f;
    [SerializeField] float _explosionRadius = 1f;

    [Header("Audio/Visual Feedback")]
    [SerializeField] Animator _pistolAnimator;
    [SerializeField] GameObject _fireFX;
    [SerializeField] GameObject _enemyHitFX;
    [SerializeField] GameObject _environmentHitFX;
    GameObject _currentFireFX;
    GameObject _currentHitFX;

    [Header("Collision Filter")]
    [SerializeField] LayerMask _hitLayers;

    private Vector3 _rayDirection;
    private bool _readyToFire = true;

    RaycastHit _objectHit;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (_readyToFire == true)
            {
                StartCoroutine(ShootRoutine());
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _pistolAnimator.SetTrigger("GunAimed");
            _pistolAnimator.SetBool("IsAiming", true);
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            _pistolAnimator.SetBool("IsAiming", false);
        }
    }

    // Rounds Per Second Timer, paces weapon rate of fire
    IEnumerator ShootRoutine()
    {
        _readyToFire = false;
        Shoot();
        yield return new WaitForSeconds(1 / _roundsPerSecond);
        _readyToFire = true;
    }

    IEnumerator DestroyRoutine(GameObject gameObject, float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }

    // fire the weapon
    void Shoot()
    {
        //Play FX
        _pistolAnimator.SetTrigger("GunFired");
        NewFireFX();

        #region Calculate Raycast direction
        // calculate direction to shoot the ray with applied inaccuracy
        Vector3 deviation = new Vector3(
            Mathf.Lerp(-_maxAngleOfInaccuracy, _maxAngleOfInaccuracy, Random.Range(0f, 1f)),
            Mathf.Lerp(-_maxAngleOfInaccuracy, _maxAngleOfInaccuracy, Random.Range(0f, 1f)),
            Mathf.Lerp(-_maxAngleOfInaccuracy, _maxAngleOfInaccuracy, Random.Range(0f, 1f)));
        _rayDirection = _cameraController.transform.forward + deviation;
        #endregion Calculate Raycast direction End

        // cast a debug ray
        Debug.DrawRay(_rayOrigin.position, _rayDirection * _shootDistance, Color.blue, 1f);

        // do the raycast
        if (Physics.Raycast(_rayOrigin.position, _rayDirection, out _objectHit, _shootDistance, _hitLayers))
        {

            // if an enemy was hit
            if (_objectHit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                NewHitFX(_enemyHitFX);
                KnockBack();

                //apply damage to interfaced enemy
                if(_objectHit.transform.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemy.ApplyDamage(_weaponDamage);
                    Debug.Log("Still using 'Enemy' instead of 'IDamageable'.");
                }

            }
            // if the environment was hit
            if (_objectHit.transform.gameObject.layer == LayerMask.NameToLayer("Environment"))
            {
                NewHitFX(_environmentHitFX);
            }

            if(_objectHit.transform.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.ApplyDamage(_weaponDamage, _objectHit.point);
            }

        }
    }

    void NewFireFX()
    {
        float destroyTimer = 0f;
        _currentFireFX = Instantiate(_fireFX, _barrelLocator.position, _barrelLocator.rotation, DynamicSingleton.Instance.transform);

        if(_currentFireFX.TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            audioSource.Play();

            if (destroyTimer < audioSource.clip.length)
            {
                destroyTimer = audioSource.clip.length;
            }
        }
        if(_currentFireFX.TryGetComponent<ParticleSystem>(out ParticleSystem particleSystem))
        {
            particleSystem.Play();

            if (destroyTimer < particleSystem.main.duration)
            {
                destroyTimer = particleSystem.main.duration;
            }
        }

        StartCoroutine(DestroyRoutine(_currentFireFX, destroyTimer));

    }

    void NewHitFX(GameObject hitFX)
    {
        _currentHitFX = Instantiate(hitFX);
        _currentHitFX.transform.position = _objectHit.point;
        _currentHitFX.transform.forward = _objectHit.normal;
        _currentHitFX.transform.SetParent(DynamicSingleton.Instance.transform);
        Color colorRemap = Color.magenta;

        if (_currentHitFX.TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            audioSource.Play();
        }
        if (_currentHitFX.TryGetComponent<ParticleSystem>(out ParticleSystem particleSystem))
        {
            Gradient cbsOriginalGradient = particleSystem.colorBySpeed.color.gradient;
            Gradient cbsNewGradient = new Gradient();
            var colorBySpeed = particleSystem.colorBySpeed;
            var shape = particleSystem.shape;

            colorRemap = cbsOriginalGradient.Evaluate(0f);
            colorBySpeed.enabled = true;
            shape.enabled = true;

            shape.rotation += _cameraController.transform.forward / 2;

            // change hitColor to match the hit object
            Renderer renderer = _objectHit.transform.gameObject.GetComponent(typeof(Renderer)) as Renderer;
            if (renderer != null)
            {
                colorRemap = renderer.material.color;
            }
            //If the parent had no color, get the first color from its children
            else
            {
                renderer = _objectHit.transform.gameObject.GetComponentInChildren(typeof(Renderer)) as Renderer;
                if (renderer != null)
                {
                    colorRemap = renderer.material.color;
                    //Debug.Log("FirePS using color of children");
                }
            }

            //Adjust _hitPS's colorBySpeed gradient
            cbsNewGradient.SetKeys
                (new GradientColorKey[] {
                new GradientColorKey(colorRemap, 0.25f),
                new GradientColorKey(cbsOriginalGradient.Evaluate(1f), 0.7f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f) });

            colorBySpeed.color = cbsNewGradient;

            //Play the effect
            particleSystem.Play();

        }
        if(_currentHitFX.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
        {
            //Create a replacement SpriteRenderer to be parented, thus avoiding scaling issues
            SpriteRenderer currentSprite = Instantiate(spriteRenderer);
            float offset = 0.02f;
            spriteRenderer.enabled = false;
            currentSprite.enabled = true;

            currentSprite.transform.localPosition += spriteRenderer.transform.forward * offset;
            currentSprite.transform.forward = -currentSprite.transform.forward;
            currentSprite.color = colorRemap;
            currentSprite.transform.SetParent(_objectHit.transform);
        }
        _currentHitFX.TryGetComponent<Animator>(out Animator animator);

    }

    void KnockBack()
    {
        if(_objectHit.transform.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
        {
            rigidbody.AddForce(_rayDirection * _knockbackMultiplier, ForceMode.Impulse);
            //Debug.Log("Knockback");
        }
    }

    void CreateExplosiveForce()
    {
        Collider[] colliders = Physics.OverlapSphere(_objectHit.point, _explosionRadius);
        foreach(Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if(rb != null)
            {
                rb.AddExplosionForce(_explosionForce, _objectHit.point, _explosionRadius, 1.0f);
                //Debug.Log("ExplosionForce applied");
            }
        }

    }
}
