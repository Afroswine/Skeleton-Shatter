using UnityEngine;

public interface IDamageable
{
    void ApplyDamage(int damageAmount, Vector3 pointOfContact);
}

public interface IHealthNew : IDamageable
{
    int MaxHealth { get; }
    int Health { get; }
    
    // set health to a new value
    void SetHealth(int newHealth);

    // actions to perform when 'health <= 0'
    void Die();
}

public interface IHealthHealable : IHealthNew
{
    void Heal(int healAmount);
}
