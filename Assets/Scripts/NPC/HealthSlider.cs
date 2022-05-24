using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthSlider : MonoBehaviour
{
    [SerializeField] Enemy _enemy;
    private Slider _slider;

    private void OnEnable()
    {
        _enemy.TookDamage.AddListener(UpdateDisplay);
        _enemy.Healed.AddListener(UpdateDisplay);
        _enemy.HealthChanged.AddListener(UpdateDisplay);
    }

    private void OnDisable()
    {
        _enemy.TookDamage.RemoveListener(UpdateDisplay);
        _enemy.Healed.RemoveListener(UpdateDisplay);
        _enemy.HealthChanged.RemoveListener(UpdateDisplay);
    }

    // Start is called before the first frame update
    void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.maxValue = _enemy.MaxHealth;
        _slider.value = _enemy.CurrentHealth;
        
    }

    public void UpdateDisplay()
    {
        _slider.value = _enemy.CurrentHealth;
    }

}
