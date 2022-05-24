using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackScriptableObject", menuName = "ScriptableObjects/EnemyAttack")]
public class EnemyAttackScriptableObject : ScriptableObject
{
    [SerializeField] 
    private float _attackDamage = 20f;
    public float AttackDamage => _attackDamage;
    [SerializeField] 
    private float _attackRange = 10f;
    public float AttackRange => _attackRange;
    [SerializeField] 
    private AudioClip _attackSound;
    public AudioClip AttackSound => _attackSound;
}
