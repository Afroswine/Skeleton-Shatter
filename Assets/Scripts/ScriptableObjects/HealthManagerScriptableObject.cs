using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "HealthManagerScriptableObject", menuName = "ScriptableObjects/Health Manager")]
public class HealthManagerScriptableObject : ScriptableObject
{
    public int _health = 100;

    [SerializeField] private int _maxHealth = 100;

    // people subscribe to this event to get notified of health changes
    [System.NonSerialized]
    public UnityEvent<int> HealthChangeEvent;

    private void OnEnable()
    {
        _health = _maxHealth;
        if(HealthChangeEvent == null)
        {
            HealthChangeEvent = new UnityEvent<int>();
        }
    }

    public void DecreaseHealth(int amount)
    {
        _health -= amount;
        HealthChangeEvent.Invoke(_health);
    }
}
