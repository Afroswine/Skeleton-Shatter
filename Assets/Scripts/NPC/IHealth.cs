using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IHealth
{
    int MaxHealth { get; }
    int StartingHealth { get; }
    int CurrentHealth { get; }
    bool IsEssential { get; }

    void ApplyDamage(int damage);
    void Die();
}
