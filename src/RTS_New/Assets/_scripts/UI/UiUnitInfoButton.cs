using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiUnitInfoButton : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] private Button _button;
    [SerializeField] private Image _health;

    private Unit _unit;
    
    public void SetButton(Unit unit, UnityAction onClick)
    {
        if (_unit)
            _unit.Health.HealthChanged -= UpdateUnitHealth;
        _unit = unit;
        _icon.sprite = _unit.Data.Icon;
        UpdateUnitHealth(_unit.Health.CurrentHealth, _unit.Data.Health);
        _unit.Health.HealthChanged += UpdateUnitHealth;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(onClick);
    }

    public void SetActive(bool setActive)
    {
        gameObject.SetActive(setActive);
    }
    
    private void UpdateUnitHealth(float current, float max)
    {
        _health.fillAmount = current / max;
    }
}