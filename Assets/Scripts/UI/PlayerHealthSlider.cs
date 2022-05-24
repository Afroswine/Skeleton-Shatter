using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class PlayerHealthSlider : MonoBehaviour
{
    [SerializeField]
    private PlayerHealth _health;

    private Slider _slider;

    private void Awake()
    {
        // get the reference
        _slider = GetComponent<Slider>();
        // set max value on slider
        _slider.maxValue = _health.MaxHealth;
        // set current value on slider
        _slider.value = _health.StartingHealth;
    }

    private void OnEnable()
    {
        _health.TookDamage.AddListener(UpdateDisplay);
    }

    private void OnDisable()
    {
        _health.TookDamage.RemoveListener(UpdateDisplay);
    }

    public void UpdateDisplay()
    {
        _slider.value = _health.CurrentHealth;
    }
}
