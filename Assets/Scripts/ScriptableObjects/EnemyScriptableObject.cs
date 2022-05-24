using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/Enemy2")]
public class EnemyScriptableObject : ScriptableObject
{
    
    [SerializeField] 
    private int _health = 100;
    public int Health => _health;
    [SerializeField] 
    private float _speed = 5f;
    public float Speed => _speed;
    [SerializeField] 
    private EnemyAttackScriptableObject _enemyAttackType;
    public EnemyAttackScriptableObject EnemyAttackType => _enemyAttackType;

    #region ScriptableObject Instantiation Example
    /*
    EnemyAttackScriptableObject _example;

    void InstantiateScriptableObject()
    {
        _example = (EnemyAttackScriptableObject)ScriptableObject.CreateInstance(typeof(EnemyAttackScriptableObject));
        Debug.Log(_example.AttackDamage);

        // don't destroy your *actual* scriptable object, bc there's only one instance of it in your project
        ScriptableObject.Destroy(_example);
    }
    */
    #endregion

    private void Awake()
    {
        Debug.Log("Awake");
    }
}
