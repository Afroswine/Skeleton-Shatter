using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private HealthManagerScriptableObject _healthManagerScriptableObject;

    private void Start()
    {
        ChangeSliderValue(_healthManagerScriptableObject._health);
    }

    private void OnEnable()
    {
        _healthManagerScriptableObject.HealthChangeEvent.AddListener(ChangeSliderValue);
    }

    private void OnDisable()
    {
        _healthManagerScriptableObject.HealthChangeEvent.RemoveListener(ChangeSliderValue);
    }

    public void ChangeSliderValue(int amount)
    {
        _slider.value = ConvertIntToFloatDecimal(amount);
    }

    private float ConvertIntToFloatDecimal(int amount)
    {
        return (float)amount / 100;
    }
}