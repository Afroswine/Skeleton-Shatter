using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour, IDamageable
{
    [Header("Broken Variant")]
    [SerializeField, Tooltip("The broken variant of this object.")]
    private Rubble _rubble;
    [SerializeField, Tooltip("The time (seconds) before this object stops being interactive.")]
    private float _activeDuration = 5f;


    public virtual void ApplyDamage(int amount, Vector3 point)
    {
        GameObject go = Instantiate(_rubble.gameObject, transform.position, transform.rotation);
        go.GetComponent<Rubble>().StartDeactivation(_activeDuration);

        ApplyForce(point);

        Destroy(gameObject);
    }

    private void ApplyForce(Vector3 point)
    {
        LayerMask layerMask = _rubble.gameObject.layer;

        Collider[] colliders = Physics.OverlapSphere(point, 0.2f, layerMask);
        foreach(Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if(rb != null)
            {
                rb.AddExplosionForce(Stats.Force.Light, point, 1f);
            }
        }
    }
}
