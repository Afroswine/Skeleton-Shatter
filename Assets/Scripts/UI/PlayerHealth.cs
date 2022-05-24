using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public UnityEvent TookDamage;
    public UnityEvent Died;
    // public property, public get private set
    [SerializeField]
    private int _maxHealth = 100;
    [SerializeField]
    private int _startingHealth = -1;
    // allow read privileges of max health
    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;
    public int StartingHealth => _startingHealth;

    // getter method example, it's just the longer
    // way to do it
    /*
    public int GetMaxHealth()
    {
        return _maxHealth;
    }
    */

    private int _currentHealth = 0;


    private void Awake()
    {
        if(_startingHealth == -1)
        {
            _startingHealth = _maxHealth;
        }

        _currentHealth = _startingHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        Debug.Log("Apply " + damageAmount + " damage");
        // ensure number is not 'negative'
        damageAmount = System.Math.Abs(damageAmount);
        // subtract damage from health
        _currentHealth -= damageAmount;
        // check to see if we've run out of health
        TookDamage.Invoke();

        if (_currentHealth <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        Died.Invoke();
        Debug.Log(gameObject.ToString() + " is dead");
    }
}
