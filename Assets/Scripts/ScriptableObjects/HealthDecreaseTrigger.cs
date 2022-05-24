using UnityEngine;

public class HealthDecreaseTrigger : MonoBehaviour
{
    [SerializeField, Tooltip("Damage to Player")]
    private int _healthDecreaseAmount = 10;
    [SerializeField]
    private HealthManagerScriptableObject _healthManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _healthManager.DecreaseHealth(_healthDecreaseAmount);
        }
    }
}
