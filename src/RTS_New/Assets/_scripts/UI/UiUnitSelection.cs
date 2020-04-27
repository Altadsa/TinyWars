using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UiUnitSelection : MonoBehaviour
{
    [SerializeField] private PlayerSelectionController _sc;
    [SerializeField] private GameObject _selectionUI;
    [SerializeField] private Transform _selection;
    private UiUnitInfoButton[] _selectionObjects;

    [Header("****INFO PANEL****")] 
    public Image Icon;

    public TMP_Text UnitName;
    
    public Image Health;

    public TMP_Text HealthText;

    public TMP_Text DamageText;

    private EntityHealth _health;

    private List<UnitHealth> _selectedUnits;
    
    void Awake()
    {
        SetObjects();
        _selectedUnits = new List<UnitHealth>();
        _sc.SelectionUpdated += SelectionUpdated;
        _selectionUI.SetActive(false);
    }

    private void SelectionUpdated(List<Entity> units)
    {
        if (units == null || units.Count == 0 || units[0] is Building)
        {
            _selectionUI.SetActive(false);
            return;
        }
        _selectionUI.SetActive(true);
        SetInfo(units[0] as Unit);
        for (int i = 0; i < units.Count; i++)
        {
            var unit = units[i] as Unit;
            
            _selectionObjects[i].SetButton(unit, delegate
            {
                SetInfo(unit);
            });
            
            _selectionObjects[i].SetActive(true);
        }

        for (int i = units.Count; i < _selectionObjects.Length; i++)
        {
            _selectionObjects[i].SetActive(false);
        }
    }

    private void SetInfo(Unit unit)
    {
        if (_health)
            _health.HealthChanged -= UpdateHealth;
        Icon.sprite = unit.Data.Icon;
        UnitName.text = unit.Data.Name;
        _health = unit.Health;
        UpdateHealth(_health.CurrentHealth, unit.Data.Health);
        _health.HealthChanged += UpdateHealth;
    }

    private void UpdateHealth(float current, float max)
    {
        Health.fillAmount = current / max;
        HealthText.text = $"{current}/{max}";
    }
    
    private void SetObjects()
    {
        _selectionObjects = new UiUnitInfoButton[_selection.childCount];
        for (int i = 0; i < _selection.childCount; i++)
        {
            var obj = _selection.GetChild(i).GetChild(0);
            _selectionObjects[i] = obj.GetComponent<UiUnitInfoButton>();
            _selectionObjects[i].SetActive(false);
        }
    }

}