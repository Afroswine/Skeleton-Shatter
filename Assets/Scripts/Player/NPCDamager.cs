using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class NPCDamager : MonoBehaviour
{
    [SerializeField]
    private int _damage = 5;

    public UnityEvent DealtDamage;

    // makes sure the gameObject's collider is a trigger
    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    /*
    // if the other component has health, deal damage
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter(Collider other)");
        if (other.TryGetComponent(out NPCHealth npcHealth))
        {
            other.gameObject.GetComponent<NPCHealth>().TakeDamage(_damage);
        }
    }
    */
}